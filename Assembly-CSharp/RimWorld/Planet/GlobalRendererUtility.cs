using System;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x0200230D RID: 8973
	public static class GlobalRendererUtility
	{
		// Token: 0x0600DD9A RID: 56730 RVA: 0x00528388 File Offset: 0x00526588
		public static void UpdateGlobalShadersParams()
		{
			int gameTime = ShaderPropertyIDs.GameTime;
			TickManager tickManager = Find.TickManager;
			int? num = (tickManager != null) ? new int?(tickManager.TicksGame) : null;
			Shader.SetGlobalFloat(gameTime, ((num != null) ? new float?((float)num.GetValueOrDefault() / 60f) : null).GetValueOrDefault());
		}
	}
}
