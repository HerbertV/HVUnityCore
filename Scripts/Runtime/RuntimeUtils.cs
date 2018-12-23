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
using System.IO;

using UnityEngine;

# if UNITY_EDITOR
using UnityEditor;
#endif

namespace HVUnity.Core
{
	/// <summary>
	/// Class RuntimeUtils
	/// 
	/// Collection of useful utility functions, also Editor only functions.
	/// All editor functions use the prefix inEditor.
	/// </summary>
	public static class RuntimeUtils
	{

		/*
		 * =========================================================================
		 *  Editor only functions
		 * =========================================================================
		 */

		#region Editor only

		/// <summary>
		/// Editor only function.
		/// 
		/// Returns the current path of a selected project folder or "Assets"
		/// </summary>
		/// <returns></returns>
		public static string inEditorSelectedProjectWindowPath()
		{
#if UNITY_EDITOR
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
#endif
			// return default
			return "Assets";
		}

		#endregion

		/*
		 * =========================================================================
		 *  Runtime functions
		 * =========================================================================
		 */


	}
}
