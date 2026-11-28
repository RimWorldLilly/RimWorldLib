using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001FE7 RID: 8167
	public class Verb_FirefoamPop : Verb
	{
		// Token: 0x17002551 RID: 9553
		// (get) Token: 0x0600CF13 RID: 53011 RVA: 0x004C4506 File Offset: 0x004C2706
		private CompExplosive Explosive
		{
			get
			{
				return base.EquipmentSource.GetComp<CompExplosive>();
			}
		}

		// Token: 0x17002552 RID: 9554
		// (get) Token: 0x0600CF14 RID: 53012 RVA: 0x004C4513 File Offset: 0x004C2713
		private CompApparelReloadable Reloadable
		{
			get
			{
				return base.EquipmentSource.GetComp<CompApparelReloadable>();
			}
		}

		// Token: 0x17002553 RID: 9555
		// (get) Token: 0x0600CF15 RID: 53013 RVA: 0x004C4520 File Offset: 0x004C2720
		private float Radius
		{
			get
			{
				return this.Explosive.ExplosiveRadius();
			}
		}

		// Token: 0x0600CF16 RID: 53014 RVA: 0x004C452D File Offset: 0x004C272D
		protected override bool TryCastShot()
		{
			return Verb_FirefoamPop.Pop(this.caster, this.Explosive, this.Reloadable);
		}

		// Token: 0x0600CF17 RID: 53015 RVA: 0x004C4546 File Offset: 0x004C2746
		public override float HighlightFieldRadiusAroundTarget(out bool needLOSToCenter)
		{
			needLOSToCenter = true;
			return this.Radius;
		}

		// Token: 0x0600CF18 RID: 53016 RVA: 0x004C4551 File Offset: 0x004C2751
		public override void DrawHighlight(LocalTargetInfo target)
		{
			base.DrawHighlightFieldRadiusAroundTarget(this.caster);
		}

		// Token: 0x0600CF19 RID: 53017 RVA: 0x004C4564 File Offset: 0x004C2764
		public static bool Pop(Thing caster, CompExplosive explosive, CompApparelReloadable reloadable)
		{
			string text;
			if (reloadable == null || explosive == null || !reloadable.CanBeUsed(out text))
			{
				return false;
			}
			explosive.StartWick(caster);
			reloadable.UsedOnce();
			return true;
		}
	}
}
