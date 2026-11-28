using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001578 RID: 5496
	public class Need_KillThirst : Need
	{
		// Token: 0x170016F8 RID: 5880
		// (get) Token: 0x060086CA RID: 34506 RVA: 0x0030B706 File Offset: 0x00309906
		protected override bool IsFrozen
		{
			get
			{
				return (float)this.pawn.ageTracker.AgeBiologicalYears < 13f || base.IsFrozen;
			}
		}

		// Token: 0x170016F9 RID: 5881
		// (get) Token: 0x060086CB RID: 34507 RVA: 0x0030B728 File Offset: 0x00309928
		public override bool ShowOnNeedList
		{
			get
			{
				return (float)this.pawn.ageTracker.AgeBiologicalYears >= 13f && base.ShowOnNeedList;
			}
		}

		// Token: 0x060086CC RID: 34508 RVA: 0x0030B74A File Offset: 0x0030994A
		public Need_KillThirst(Pawn newPawn) : base(newPawn)
		{
			this.threshPercents = new List<float>
			{
				0.3f
			};
		}

		// Token: 0x060086CD RID: 34509 RVA: 0x0030B769 File Offset: 0x00309969
		public override void NeedInterval()
		{
			if (!this.IsFrozen)
			{
				this.CurLevel -= 8.333333E-05f;
			}
		}

		// Token: 0x060086CE RID: 34510 RVA: 0x0030B788 File Offset: 0x00309988
		public void Notify_KilledPawn(DamageInfo? dinfo)
		{
			if (dinfo == null)
			{
				return;
			}
			if (((dinfo != null) ? dinfo.GetValueOrDefault().WeaponBodyPartGroup : null) != null || ((dinfo != null) ? dinfo.GetValueOrDefault().WeaponLinkedHediff : null) != null || (dinfo.Value.Weapon != null && dinfo.Value.Weapon.IsMeleeWeapon))
			{
				this.CurLevel = 1f;
			}
		}

		// Token: 0x04005329 RID: 21289
		public const float FallPerDay = 0.033333335f;

		// Token: 0x0400532A RID: 21290
		private const float MinAgeForNeed = 13f;
	}
}
