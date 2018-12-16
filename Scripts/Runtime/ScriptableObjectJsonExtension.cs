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
using UnityEngine;

namespace HVUnity.Core
{
	/// <summary>
	/// Class ScriptableObjectJsonExtension
	/// 
	/// extension for Json Shortcut
	/// </summary>
	public static class ScriptableObjectJsonExtension
	{
		/// <summary>
		/// Returns Json string
		/// </summary>
		/// <param name="so"></param>
		/// <returns></returns>
		public static string ToJson(
				this ScriptableObject so 
			)
		{
			return JsonUtility.ToJson(so);
		}

		/// <summary>
		/// loads Json string into existing <see cref="ScriptableObject"/>
		/// </summary>
		/// <param name="so"></param>
		/// <param name="json"></param>
		public static void FromJson(
				this ScriptableObject so, 
				string json
			)
		{
			JsonUtility.FromJsonOverwrite(json, so);
		}
	}
}
