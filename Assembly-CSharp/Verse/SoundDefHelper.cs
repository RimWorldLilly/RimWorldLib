using System;
using RimWorld.Planet;
using Verse.Sound;

namespace Verse
{
	// Token: 0x020001BF RID: 447
	public static class SoundDefHelper
	{
		// Token: 0x06000C54 RID: 3156 RVA: 0x00041091 File Offset: 0x0003F291
		public static bool NullOrUndefined(this SoundDef def)
		{
			return def == null || def.isUndefined;
		}

		// Token: 0x06000C55 RID: 3157 RVA: 0x000410A0 File Offset: 0x0003F2A0
		public static bool CorrectContextNow(SoundDef def, Map sourceMap)
		{
			if (sourceMap != null && (Find.CurrentMap != sourceMap || WorldRendererUtility.WorldSelected))
			{
				return false;
			}
			switch (def.context)
			{
			case SoundContext.Any:
				return true;
			case SoundContext.MapOnly:
				return Current.ProgramState == ProgramState.Playing && WorldRendererUtility.DrawingMap;
			case SoundContext.WorldOnly:
				return WorldRendererUtility.WorldSelected;
			default:
				throw new NotImplementedException();
			}
		}
	}
}
