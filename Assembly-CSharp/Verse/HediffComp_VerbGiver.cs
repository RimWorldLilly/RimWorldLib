using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x02000487 RID: 1159
	public class HediffComp_VerbGiver : HediffComp, IVerbOwner
	{
		// Token: 0x17000614 RID: 1556
		// (get) Token: 0x06002040 RID: 8256 RVA: 0x000C237A File Offset: 0x000C057A
		public HediffCompProperties_VerbGiver Props
		{
			get
			{
				return (HediffCompProperties_VerbGiver)this.props;
			}
		}

		// Token: 0x17000615 RID: 1557
		// (get) Token: 0x06002041 RID: 8257 RVA: 0x000C2387 File Offset: 0x000C0587
		public VerbTracker VerbTracker
		{
			get
			{
				return this.verbTracker;
			}
		}

		// Token: 0x17000616 RID: 1558
		// (get) Token: 0x06002042 RID: 8258 RVA: 0x000C238F File Offset: 0x000C058F
		public List<VerbProperties> VerbProperties
		{
			get
			{
				return this.Props.verbs;
			}
		}

		// Token: 0x17000617 RID: 1559
		// (get) Token: 0x06002043 RID: 8259 RVA: 0x000C239C File Offset: 0x000C059C
		public List<Tool> Tools
		{
			get
			{
				return this.Props.tools;
			}
		}

		// Token: 0x17000618 RID: 1560
		// (get) Token: 0x06002044 RID: 8260 RVA: 0x000C23A9 File Offset: 0x000C05A9
		Thing IVerbOwner.ConstantCaster
		{
			get
			{
				return base.Pawn;
			}
		}

		// Token: 0x17000619 RID: 1561
		// (get) Token: 0x06002045 RID: 8261 RVA: 0x000C23B1 File Offset: 0x000C05B1
		ImplementOwnerTypeDef IVerbOwner.ImplementOwnerTypeDef
		{
			get
			{
				return this.Props.ownerTypeOverride ?? ImplementOwnerTypeDefOf.Hediff;
			}
		}

		// Token: 0x06002046 RID: 8262 RVA: 0x000C23C7 File Offset: 0x000C05C7
		public HediffComp_VerbGiver()
		{
			this.verbTracker = new VerbTracker(this);
		}

		// Token: 0x06002047 RID: 8263 RVA: 0x000C23DB File Offset: 0x000C05DB
		public override void CompExposeData()
		{
			base.CompExposeData();
			Scribe_Deep.Look<VerbTracker>(ref this.verbTracker, "verbTracker", new object[]
			{
				this
			});
			if (Scribe.mode == LoadSaveMode.PostLoadInit && this.verbTracker == null)
			{
				this.verbTracker = new VerbTracker(this);
			}
		}

		// Token: 0x06002048 RID: 8264 RVA: 0x000C2419 File Offset: 0x000C0619
		public override void CompPostTick(ref float _)
		{
			this.verbTracker.VerbsTick();
		}

		// Token: 0x06002049 RID: 8265 RVA: 0x000C2428 File Offset: 0x000C0628
		string IVerbOwner.UniqueVerbOwnerID()
		{
			return this.parent.GetUniqueLoadID() + "_" + this.parent.comps.IndexOf(this).ToString();
		}

		// Token: 0x0600204A RID: 8266 RVA: 0x000C2463 File Offset: 0x000C0663
		bool IVerbOwner.VerbsStillUsableBy(Pawn p)
		{
			return p.health.hediffSet.hediffs.Contains(this.parent);
		}

		// Token: 0x0400190C RID: 6412
		public VerbTracker verbTracker;
	}
}
