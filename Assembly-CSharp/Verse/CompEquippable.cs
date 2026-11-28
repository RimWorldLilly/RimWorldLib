using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;

namespace Verse
{
	// Token: 0x02000540 RID: 1344
	public class CompEquippable : ThingComp, IVerbOwner
	{
		// Token: 0x170007E6 RID: 2022
		// (get) Token: 0x06002779 RID: 10105 RVA: 0x000F681C File Offset: 0x000F4A1C
		protected Pawn Holder
		{
			get
			{
				ThingWithComps parent = this.parent;
				Pawn_EquipmentTracker pawn_EquipmentTracker = ((parent != null) ? parent.ParentHolder : null) as Pawn_EquipmentTracker;
				if (pawn_EquipmentTracker == null)
				{
					return null;
				}
				return pawn_EquipmentTracker.pawn;
			}
		}

		// Token: 0x170007E7 RID: 2023
		// (get) Token: 0x0600277A RID: 10106 RVA: 0x000F6840 File Offset: 0x000F4A40
		public List<Verb> AllVerbs
		{
			get
			{
				return this.verbTracker.AllVerbs;
			}
		}

		// Token: 0x170007E8 RID: 2024
		// (get) Token: 0x0600277B RID: 10107 RVA: 0x000F684D File Offset: 0x000F4A4D
		public Verb PrimaryVerb
		{
			get
			{
				return this.verbTracker.PrimaryVerb;
			}
		}

		// Token: 0x170007E9 RID: 2025
		// (get) Token: 0x0600277C RID: 10108 RVA: 0x000F685A File Offset: 0x000F4A5A
		public VerbTracker VerbTracker
		{
			get
			{
				return this.verbTracker;
			}
		}

		// Token: 0x170007EA RID: 2026
		// (get) Token: 0x0600277D RID: 10109 RVA: 0x000F6862 File Offset: 0x000F4A62
		public List<VerbProperties> VerbProperties
		{
			get
			{
				return this.parent.def.Verbs;
			}
		}

		// Token: 0x170007EB RID: 2027
		// (get) Token: 0x0600277E RID: 10110 RVA: 0x000F6874 File Offset: 0x000F4A74
		public List<Tool> Tools
		{
			get
			{
				return this.parent.def.tools;
			}
		}

		// Token: 0x170007EC RID: 2028
		// (get) Token: 0x0600277F RID: 10111 RVA: 0x000F6886 File Offset: 0x000F4A86
		Thing IVerbOwner.ConstantCaster
		{
			get
			{
				return null;
			}
		}

		// Token: 0x170007ED RID: 2029
		// (get) Token: 0x06002780 RID: 10112 RVA: 0x000F6889 File Offset: 0x000F4A89
		ImplementOwnerTypeDef IVerbOwner.ImplementOwnerTypeDef
		{
			get
			{
				return ImplementOwnerTypeDefOf.Weapon;
			}
		}

		// Token: 0x06002781 RID: 10113 RVA: 0x000F6890 File Offset: 0x000F4A90
		public CompEquippable()
		{
			this.verbTracker = new VerbTracker(this);
		}

		// Token: 0x06002782 RID: 10114 RVA: 0x000F68A4 File Offset: 0x000F4AA4
		public IEnumerable<Command> GetVerbsCommands()
		{
			return this.verbTracker.GetVerbsCommands();
		}

		// Token: 0x06002783 RID: 10115 RVA: 0x000F68B4 File Offset: 0x000F4AB4
		public override void PostDestroy(DestroyMode mode, Map previousMap)
		{
			base.PostDestroy(mode, previousMap);
			if (this.Holder != null && this.Holder.equipment != null && this.Holder.equipment.Primary == this.parent)
			{
				this.Holder.equipment.Notify_PrimaryDestroyed();
			}
		}

		// Token: 0x06002784 RID: 10116 RVA: 0x000F6906 File Offset: 0x000F4B06
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Deep.Look<VerbTracker>(ref this.verbTracker, "verbTracker", new object[]
			{
				this
			});
		}

		// Token: 0x06002785 RID: 10117 RVA: 0x000F6928 File Offset: 0x000F4B28
		public override void CompTick()
		{
			this.verbTracker.VerbsTick();
		}

		// Token: 0x06002786 RID: 10118 RVA: 0x000F6938 File Offset: 0x000F4B38
		public override void Notify_Unequipped(Pawn p)
		{
			List<Verb> allVerbs = this.AllVerbs;
			for (int i = 0; i < allVerbs.Count; i++)
			{
				allVerbs[i].Notify_EquipmentLost();
			}
		}

		// Token: 0x06002787 RID: 10119 RVA: 0x000F6969 File Offset: 0x000F4B69
		public virtual IEnumerable<Gizmo> CompGetEquippedGizmosExtra()
		{
			return Enumerable.Empty<Gizmo>();
		}

		// Token: 0x06002788 RID: 10120 RVA: 0x000F6970 File Offset: 0x000F4B70
		string IVerbOwner.UniqueVerbOwnerID()
		{
			return "CompEquippable_" + this.parent.ThingID;
		}

		// Token: 0x06002789 RID: 10121 RVA: 0x000F6988 File Offset: 0x000F4B88
		bool IVerbOwner.VerbsStillUsableBy(Pawn p)
		{
			Apparel apparel = this.parent as Apparel;
			if (apparel != null)
			{
				return p.apparel.WornApparel.Contains(apparel);
			}
			return p.equipment.AllEquipmentListForReading.Contains(this.parent);
		}

		// Token: 0x04001C4C RID: 7244
		public VerbTracker verbTracker;
	}
}
