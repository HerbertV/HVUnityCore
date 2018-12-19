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
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace HVUnity.Core.Editor
{
	/// <summary>
	/// Class EditorIcons
	/// 
	/// Shortcuts to access icon resources
	/// </summary>
	public static class EditorIcons
	{
		/*
		 * =========================================================================
		 *  Variables
		 * =========================================================================
		 */

		#region csharp script icons

		/// <summary>
		/// Generic Unity build-in csharp icon
		/// </summary>
		private static Texture2D csharpGenericIcon;


		private static Texture2D csharpBehaviourIcon;

		private static Texture2D csharpAbtractIcon;

		private static Texture2D csharpExtensionIcon;

		private static Texture2D csharpInterfaceIcon;

		private static Texture2D csharpSingletonIcon;

		#endregion

		#region other icons

		/// <summary>
		/// Icon for Scriptable settings objects
		/// </summary>
		private static Texture2D scriptableSettingsIcon;

		#endregion

		/*
		 * =========================================================================
		 *  Accessors
		 * =========================================================================
		 */

		/// <summary>
		/// Generic Unity build-in csharp icon
		/// </summary>
		public static Texture2D CSharpGenericIcon
		{
			get
			{
				if( csharpGenericIcon == null )
					csharpGenericIcon = (Texture2D)EditorGUIUtility.IconContent("cs Script Icon").image;

				return csharpGenericIcon;
			}
		}

		/// <summary>
		/// Icon for Scriptable settings objects
		/// </summary>
		public static Texture2D CSharpBehaviourIcon
		{
			get
			{
				if( csharpBehaviourIcon == null )
				{
					csharpBehaviourIcon = (Texture2D)AssetDatabase.LoadAssetAtPath(
							"Assets/TheWorkshop/HVUnityCore/Icons/Editor/CSharp_Behaviour.png",
							typeof(Texture2D)
						);

					// TODO package path
				}
				return csharpBehaviourIcon;
			}
		}

		/// <summary>
		/// Icon for Scriptable settings objects
		/// </summary>
		public static Texture2D ScriptableSettingsIcon
		{
			get
			{
				if( scriptableSettingsIcon == null )
				{
					scriptableSettingsIcon = (Texture2D)AssetDatabase.LoadAssetAtPath(
							"Assets/TheWorkshop/HVUnityCore/Icons/Editor/ScriptableSettings.png",
							typeof(Texture2D)
						);

					// TODO package path
				}
				return scriptableSettingsIcon;
			}
		}

		/*
		 * =========================================================================
		 *  Function
		 * =========================================================================
		 */

		public static void injectIcon(Object obj, Texture2D icon)
		{
			if( icon == null )
				return;

			PropertyInfo info = typeof(SerializedObject).GetProperty(
					"inspectorMode",
					BindingFlags.NonPublic | BindingFlags.Instance
				);
			SerializedObject serializedObject = new SerializedObject(obj);

			info.SetValue(serializedObject, InspectorMode.Debug, null);

			SerializedProperty iconProperty = serializedObject.FindProperty("m_Icon");
			iconProperty.objectReferenceValue = icon;

			serializedObject.ApplyModifiedProperties();
			serializedObject.Update();

			EditorUtility.SetDirty(obj);
		}
	}
}
