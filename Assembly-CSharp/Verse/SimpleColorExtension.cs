using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020006E7 RID: 1767
	public static class SimpleColorExtension
	{
		// Token: 0x060037E5 RID: 14309 RVA: 0x00165E64 File Offset: 0x00164064
		public static Color ToUnityColor(this SimpleColor color)
		{
			switch (color)
			{
			case SimpleColor.White:
				return Color.white;
			case SimpleColor.Red:
				return Color.red;
			case SimpleColor.Green:
				return Color.green;
			case SimpleColor.Blue:
				return Color.blue;
			case SimpleColor.Magenta:
				return Color.magenta;
			case SimpleColor.Yellow:
				return Color.yellow;
			case SimpleColor.Cyan:
				return Color.cyan;
			case SimpleColor.Orange:
				return ColorLibrary.Orange;
			default:
				throw new ArgumentException();
			}
		}
	}
}
