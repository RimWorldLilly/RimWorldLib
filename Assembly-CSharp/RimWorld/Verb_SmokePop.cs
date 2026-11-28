using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001FF1 RID: 8177
	public class Verb_SmokePop : Verb
	{
		// Token: 0x0600CF48 RID: 53064 RVA: 0x004C580F File Offset: 0x004C3A0F
		protected override bool TryCastShot()
		{
			Verb_SmokePop.Pop(base.ReloadableCompSource);
			return true;
		}

		// Token: 0x0600CF49 RID: 53065 RVA: 0x004C581D File Offset: 0x004C3A1D
		public override float HighlightFieldRadiusAroundTarget(out bool needLOSToCenter)
		{
			needLOSToCenter = false;
			return base.EquipmentSource.GetStatValue(StatDefOf.PackRadius, true, -1);
		}

		// Token: 0x0600CF4A RID: 53066 RVA: 0x004C5834 File Offset: 0x004C3A34
		public override void DrawHighlight(LocalTargetInfo target)
		{
			base.DrawHighlightFieldRadiusAroundTarget(this.caster);
		}

		// Token: 0x0600CF4B RID: 53067 RVA: 0x004C5848 File Offset: 0x004C3A48
		public static void Pop(CompApparelReloadable comp)
		{
			string text;
			if (comp == null || !comp.CanBeUsed(out text))
			{
				return;
			}
			Thing parent = comp.parent;
			Pawn wearer = comp.Wearer;
			float statValue = parent.GetStatValue(StatDefOf.PackRadius, true, -1);
			GenExplosion.DoExplosion(wearer.Position, wearer.Map, statValue, DamageDefOf.Smoke, null, -1, -1f, null, null, null, null, null, 0f, 1, new GasType?(GasType.BlindSmoke), null, 255, false, null, 0f, 1, 0f, false, null, null, null, true, 1f, 0f, true, null, 1f, null, null, null, null);
			comp.UsedOnce();
		}
	}
}
