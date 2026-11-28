using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000581 RID: 1409
	public class Graphic_Terrain : Graphic_Single
	{
		// Token: 0x060029B4 RID: 10676 RVA: 0x000FF500 File Offset: 0x000FD700
		public override string ToString()
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(31, 3);
			defaultInterpolatedStringHandler.AppendLiteral("Terrain(path=");
			defaultInterpolatedStringHandler.AppendFormatted(this.path);
			defaultInterpolatedStringHandler.AppendLiteral(", shader=");
			defaultInterpolatedStringHandler.AppendFormatted<Shader>(base.Shader);
			defaultInterpolatedStringHandler.AppendLiteral(", color=");
			defaultInterpolatedStringHandler.AppendFormatted<Color>(this.color);
			defaultInterpolatedStringHandler.AppendLiteral(")");
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}
	}
}
