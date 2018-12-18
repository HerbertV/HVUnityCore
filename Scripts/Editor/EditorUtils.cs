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

using UnityEditor;
using UnityEngine;

namespace HVUnity.Core.Editor
{
	/// <summary>
	/// Class EditorUtils
	/// 
	/// Collection of useful editor utilities.
	/// </summary>
	public static class EditorUtils
	{
		/// <summary>
		/// returns the current path of a selected project folder or "Assets"
		/// </summary>
		/// <returns></returns>
		public static string getSelectedProjectWindowPath()
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

		
	}
}
