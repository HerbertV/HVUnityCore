/*  __  __      
 * /\ \/\ \  __________   
 * \ \ \_\ \/_______  /\   
 *  \ \  _  \  ____/ / /   
 *   \ \_\ \_\ \ \/ / /    
 *    \/_/\/_/\ \ \/ /     Copyright (c) 2018 Herbert Veitengruber
 *             \ \  /      https://github.com/HerbertV
 *              \_\/
 * -----------------------------------------------------------------------------
 *  Licensed under the MIT License. 
 *  See LICENSE file in the project root for full license information.
 */
using System.Linq;
using System.Collections.Generic;

using UnityEngine;

#if UNITY_EDITOR

using System.IO;
using UnityEditor;

#endif

namespace HVUnity.Core
{
	/// <summary>
	/// Abstract Class SingletonScriptableObject 
	/// for making reload-proof singletons out of ScriptableObjects
	/// 
	/// Returns the asset created on editor, null if there is none
	/// Based on https://www.youtube.com/watch?v=VBA1QCoEAX4
	/// 
	/// Extended with:
	/// - preload in player feature 
	/// - thread lock 
	/// - available check 
	/// - instance creation 
	/// </summary>
	/// <typeparam name="T">Type of the singleton</typeparam>
	public abstract class SingletonScriptableObject<T> : ScriptableObject 
			where T : ScriptableObject
	{
		/*
		 * =========================================================================
		 *  Variables
		 * =========================================================================
		 */
		
		/// <summary>
		/// singlton instance
		/// </summary>
		private static T instance = null;

		/// <summary>
		/// Thread save lock object
		/// </summary>
		private static object obj = new object();
		
		/// <summary>
		/// If true this object is added to the players preload array
		/// </summary>
		protected bool preloadInPlayer = false;

		/*
		 * =========================================================================
		 *  Accessors
		 * =========================================================================
		 */

		/// <summary>
		/// Instance accessor
		/// </summary>
		public static T Instance
		{
			get
			{
				lock( obj )
				{
					if( !instance )
					{
#if UNITY_EDITOR
						string guid = AssetDatabase.FindAssets("t:" + typeof(T).Name).FirstOrDefault();

						instance = AssetDatabase.LoadAssetAtPath<T>(
								AssetDatabase.GUIDToAssetPath(guid)
							);
#else
						instance = Resources.FindObjectsOfTypeAll<T>().FirstOrDefault();
#endif
					}
				}
				return instance;
			}
		}
			
		/// <summary>
		/// Check if the singleton is available as asset.
		/// 
		/// Can be used for validation menu items, to prevent multiple singleton creations
		/// </summary>
		/// <returns></returns>
		public static bool IsAvailable
		{
			get
			{ 
#if UNITY_EDITOR
				if( AssetDatabase.FindAssets("t:" + typeof(T).Name).FirstOrDefault() != null )
					return true;
#else
				if( Resources.FindObjectsOfTypeAll<T>().FirstOrDefault() != null )
					return true;
#endif

				return false;
			}
		}

		/*
		 * =========================================================================
		 *  Unity3d Behaviour Live Cycle
		 * =========================================================================
		 */

		protected virtual void Awake()
		{
#if UNITY_EDITOR
			if( preloadInPlayer )
			{
				List<Object> preloadedAssets = PlayerSettings.GetPreloadedAssets().ToList();
				if( !preloadedAssets.Contains(this) )
				{
					preloadedAssets.Add(this);
					PlayerSettings.SetPreloadedAssets(preloadedAssets.ToArray());
				}
			}
#endif
		}

		protected virtual void OnDestroy()
		{
#if UNITY_EDITOR
			if( preloadInPlayer )
			{
				List<Object> preloadedAssets = PlayerSettings.GetPreloadedAssets().ToList();
				if( preloadedAssets.Contains(this) )
				{
					preloadedAssets.Remove(this);
					PlayerSettings.SetPreloadedAssets(preloadedAssets.ToArray());
				}
			}
#endif
			instance = null;
		}
		
#if UNITY_EDITOR

		/// <summary>
		/// Creates the instance and saves it as asset.
		/// </summary>
		/// <returns></returns>
		public static T createSingletonInstance()
		{
			if( instance != null || IsAvailable )
				return default(T);
			
			T asset = CreateInstance<T>();

			string path = getAssetSelectionPath();

			string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath(path + "/" + typeof(T).Name + ".asset");

			AssetDatabase.CreateAsset(asset, assetPathAndName);
			AssetDatabase.SaveAssets();
			EditorUtility.FocusProjectWindow();
			Selection.activeObject = asset;

			return asset;
		}

		private static string getAssetSelectionPath()
		{
			Object obj = Selection.activeObject;

			if( obj == null )
				return "Assets";

			string path = AssetDatabase.GetAssetPath(obj);

			if( !string.IsNullOrEmpty(path) )
			{
				if( File.Exists(path) )
					return Path.GetDirectoryName(path);
				else
					return path;
			}
			// return default
			return "Assets";
		}
#endif

	}
}
