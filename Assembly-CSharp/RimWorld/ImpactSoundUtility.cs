using System;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001F25 RID: 7973
	public static class ImpactSoundUtility
	{
		// Token: 0x0600CB87 RID: 52103 RVA: 0x004AFE78 File Offset: 0x004AE078
		public static void PlayImpactSound(Thing hitThing, ImpactSoundTypeDef ist, Map map)
		{
			if (ist == null)
			{
				return;
			}
			if (ist.playOnlyIfHitPawn && !(hitThing is Pawn))
			{
				return;
			}
			if (map == null)
			{
				Log.Warning("Can't play impact sound because map is null.");
				return;
			}
			SoundDef soundDef;
			if (hitThing is Pawn)
			{
				soundDef = ist.soundDef;
			}
			else
			{
				if (hitThing.Stuff != null)
				{
					if (ist == ImpactSoundTypeDefOf.Bullet)
					{
						soundDef = hitThing.Stuff.stuffProps.soundImpactBullet;
					}
					else
					{
						soundDef = hitThing.Stuff.stuffProps.soundImpactMelee;
					}
				}
				else
				{
					soundDef = hitThing.def.soundImpactDefault;
				}
				if (soundDef.NullOrUndefined())
				{
					soundDef = SoundDefOf.Pawn_Melee_Punch_HitBuilding_Generic;
				}
			}
			if (!soundDef.NullOrUndefined())
			{
				soundDef.PlayOneShot(new TargetInfo(hitThing.PositionHeld, map, false));
			}
		}
	}
}
