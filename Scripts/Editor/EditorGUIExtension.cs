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
using System.Collections.Generic;

using UnityEditor;
using UnityEngine;

namespace HVUnity.Core.Editor
{
	/// <summary>
	/// Class EditorGUIExtension
	/// 
	/// Extensions for the Unity Editor GUI
	/// </summary>
	static public class EditorGUIExtension
	{
		/*
		 * =========================================================================
		 *  Variables
		 * =========================================================================
		 */

		/// <summary>
		/// default dark background color for editor header elements
		/// </summary>
		public readonly static Color colorBackgroundDark = new Color32(72, 72, 72, 255);

		/// <summary>
		/// default light background color for editor header elements
		/// </summary>
		public readonly static Color colorBackgroundLight = new Color32(144, 144, 144, 255);

		/// <summary>
		/// for foldouts with bold label
		/// </summary>
		private static GUIStyle styleFoldoutBold;

		/// <summary>
		/// style of the horizontal seperator line with bottom margin
		/// </summary>
		private static GUIStyle styleLine;

		/// <summary>
		/// style of the horizontal seperator line without margins
		/// </summary>
		private static GUIStyle styleLineNoMargin;

		/// <summary>
		/// draw the background in a bit darker color
		/// </summary>
		private static GUIStyle styleBackgroundDark;

		/// <summary>
		/// draw the background in a bit lighter color
		/// </summary>
		private static GUIStyle styleBackgroundLight;

		private static Stack<Color> preservedColors = new Stack<Color>();

		/*
		 * =========================================================================
		 *  Accessors
		 * =========================================================================
		 */

		/// <summary>
		/// for foldouts with bold label
		/// </summary>
		public static GUIStyle StyleFoldoutBold
		{
			get
			{
				if( styleFoldoutBold == null )
				{
					styleFoldoutBold = new GUIStyle(EditorStyles.foldout);
					styleFoldoutBold.font = new GUIStyle(EditorStyles.boldLabel).font;
				}

				return styleFoldoutBold;
			}
		}

		/// <summary>
		/// style of the horizontal seperator line with bottom margin
		/// </summary>
		public static GUIStyle StyleLine
		{
			get
			{
				if( styleLine == null )
				{
					styleLine = new GUIStyle("Box");
					styleLine.border.top = styleLine.border.bottom = 1;
					styleLine.margin.left = styleLine.margin.right = 0;
				}

				return styleLine;
			}
		}

		/// <summary>
		/// style of the horizontal seperator line without margins
		/// </summary>
		public static GUIStyle StyleLineNoMargin
		{
			get
			{
				if( styleLineNoMargin == null )
				{
					styleLineNoMargin = new GUIStyle("Box");
					styleLineNoMargin.border.top = styleLineNoMargin.border.bottom = 1;
					styleLineNoMargin.margin.left = styleLineNoMargin.margin.right = 0;
					styleLineNoMargin.margin.top = styleLineNoMargin.margin.bottom = 0;
				}

				return styleLineNoMargin;
			}
		}
				
		public static GUIStyle StyleBackgroundDark
		{
			get
			{
				if( styleBackgroundDark == null )
				{
					styleBackgroundDark = new GUIStyle();
					styleBackgroundDark.normal.background = makeTexture(1, 1, colorBackgroundDark);
				}

				return styleBackgroundDark;
			}
		}

		public static GUIStyle StyleBackgroundLight
		{
			get
			{
				if( styleBackgroundLight == null )
				{
					styleBackgroundLight = new GUIStyle();
					styleBackgroundLight.normal.background = makeTexture(1, 1, colorBackgroundLight);
				}

				return styleBackgroundLight;
			}
		}



		/*
		 * =========================================================================
		 *  Functions
		 * =========================================================================
		 */

		/// <summary>
		/// helper to for making a background Texture
		/// </summary>
		/// <param name="width"></param>
		/// <param name="height"></param>
		/// <param name="col"></param>
		/// <returns></returns>
		public static Texture2D makeTexture(int width, int height, Color col)
		{
			Color[] pix = new Color[width * height];

			for( int i = 0; i < pix.Length; i++ )
				pix[i] = col;

			Texture2D result = new Texture2D(width, height);
			result.SetPixels(pix);
			result.Apply();

			return result;
		}

		/// <summary>
		/// Draws a horizontal line to seperate gui elements in Inspector or Editor Windows
		/// </summary>
		/// <param name="c">optional line color</param>
		/// <param name="nomargin">if true margin is disabled</param>
		public static void horizontalLine(Color? c = null, bool nomargin = false)
		{			
			if( c != null )
			{
				preservedColors.Push(GUI.backgroundColor);
				GUI.backgroundColor = (Color)c;
			}

			GUIStyle s = StyleLine;

			if( nomargin )
				s = StyleLineNoMargin;

			GUILayout.Box(GUIContent.none, s, GUILayout.ExpandWidth(true), GUILayout.Height(1));

			if( c != null )
			{
				GUI.backgroundColor = preservedColors.Pop();
			}
		}

		/// <summary>
		/// Default foldout with bold label
		/// </summary>
		/// <param name="fold"></param>
		/// <param name="title"></param>
		/// <returns></returns>
		public static bool foldoutBold(bool fold, string title)
		{
			fold = EditorGUILayout.Foldout(fold, title, StyleFoldoutBold);
			return fold;
		}

		/// <summary>
		/// foldout section with headerlike background and bold text
		/// </summary>
		/// <param name="fold"></param>
		/// <param name="title"></param>
		/// <returns></returns>
		public static bool foldoutSection(bool fold, string title)
		{
			GUIStyle bg = new GUIStyle();
			
			if( EditorGUIUtility.isProSkin )
			{
				bg = StyleBackgroundDark;
			} else {
				bg = StyleBackgroundLight;
			}

			horizontalLine(Color.white, true);

			GUILayout.BeginHorizontal(bg);
			fold = foldoutBold(fold, title);
			GUILayout.EndHorizontal();

			horizontalLine(Color.black, true);

			return fold;
		}

		public static void headerSection(string title)
		{
			GUIStyle bg = new GUIStyle();

			if( EditorGUIUtility.isProSkin )
			{
				bg = StyleBackgroundDark;
			} else {
				bg = StyleBackgroundLight;
			}

			horizontalLine(Color.white, true);

			GUILayout.BeginHorizontal(bg);
			EditorGUILayout.LabelField(title, EditorStyles.boldLabel);
			GUILayout.EndHorizontal();

			horizontalLine(Color.black, true);
		}
	}
}
