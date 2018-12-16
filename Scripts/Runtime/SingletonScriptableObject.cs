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

namespace HVUnity.Core
{
	/// <summary>
	/// Abstract Class SingletonScriptableObject 
	/// for making reload-proof singletons out of ScriptableObjects
	/// 
	/// Returns the asset created on editor, null if there is none
	/// Based on https://www.youtube.com/watch?v=VBA1QCoEAX4
	/// Added preload in player feature and thread lock
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
						string guid = UnityEditor.AssetDatabase.FindAssets("t:" + typeof(T).Name).FirstOrDefault();

						instance = UnityEditor.AssetDatabase.LoadAssetAtPath<T>(
								UnityEditor.AssetDatabase.GUIDToAssetPath(guid)
							);
#else
						instance = Resources.FindObjectsOfTypeAll<T>().FirstOrDefault();
#endif
					}
				}
				return instance;
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
				List<Object> preloadedAssets = UnityEditor.PlayerSettings.GetPreloadedAssets().ToList();
				if( !preloadedAssets.Contains(this) )
				{
					preloadedAssets.Add(this);
					UnityEditor.PlayerSettings.SetPreloadedAssets(preloadedAssets.ToArray());
				}
			}
#endif
		}

		protected virtual void OnDestroy()
		{
#if UNITY_EDITOR
			if( preloadInPlayer )
			{
				List<Object> preloadedAssets = UnityEditor.PlayerSettings.GetPreloadedAssets().ToList();
				if( preloadedAssets.Contains(this) )
				{
					preloadedAssets.Remove(this);
					UnityEditor.PlayerSettings.SetPreloadedAssets(preloadedAssets.ToArray());
				}
			}
#endif
			instance = null;
		}
	}
}
