using System;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld
{
	// Token: 0x020019E4 RID: 6628
	public abstract class CompEntityHolder : ThingComp
	{
		// Token: 0x17001D33 RID: 7475
		// (get) Token: 0x0600A6E5 RID: 42725 RVA: 0x003B7ACB File Offset: 0x003B5CCB
		public virtual float ContainmentStrength
		{
			get
			{
				return this.parent.GetStatValue(StatDefOf.ContainmentStrength, true, 15);
			}
		}

		// Token: 0x17001D34 RID: 7476
		// (get) Token: 0x0600A6E6 RID: 42726 RVA: 0x003B7AE0 File Offset: 0x003B5CE0
		public CompProperties_EntityHolder Props
		{
			get
			{
				return (CompProperties_EntityHolder)this.props;
			}
		}

		// Token: 0x17001D35 RID: 7477
		// (get) Token: 0x0600A6E7 RID: 42727 RVA: 0x003B7AED File Offset: 0x003B5CED
		protected Building_HoldingPlatform HoldingPlatform
		{
			get
			{
				return (Building_HoldingPlatform)this.parent;
			}
		}

		// Token: 0x17001D36 RID: 7478
		// (get) Token: 0x0600A6E8 RID: 42728
		public abstract bool Available { get; }

		// Token: 0x17001D37 RID: 7479
		// (get) Token: 0x0600A6E9 RID: 42729
		public abstract Pawn HeldPawn { get; }

		// Token: 0x17001D38 RID: 7480
		// (get) Token: 0x0600A6EA RID: 42730
		public abstract ThingOwner Container { get; }

		// Token: 0x0600A6EB RID: 42731
		public abstract void EjectContents();

		// Token: 0x0600A6EC RID: 42732 RVA: 0x003B7AFC File Offset: 0x003B5CFC
		public override string CompInspectStringExtra()
		{
			string text = base.CompInspectStringExtra();
			if (!text.NullOrEmpty())
			{
				text += "\n";
			}
			float statValue = this.parent.GetStatValue(StatDefOf.ContainmentStrength, true, -1);
			string str = text;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(2, 2);
			defaultInterpolatedStringHandler.AppendFormatted<TaggedString>(StatDefOf.ContainmentStrength.LabelCap);
			defaultInterpolatedStringHandler.AppendLiteral(": ");
			defaultInterpolatedStringHandler.AppendFormatted<float>(statValue, "F0");
			text = str + defaultInterpolatedStringHandler.ToStringAndClear();
			if (!this.parent.Spawned)
			{
				return text;
			}
			if (this.parent.IsOutside())
			{
				string str2 = text;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2 = new DefaultInterpolatedStringHandler(3, 1);
				defaultInterpolatedStringHandler2.AppendLiteral(" (");
				defaultInterpolatedStringHandler2.AppendFormatted<TaggedString>("Outdoors".Translate());
				defaultInterpolatedStringHandler2.AppendLiteral(")");
				text = str2 + defaultInterpolatedStringHandler2.ToStringAndClear();
			}
			else if (StatWorker_ContainmentStrength.AnyDoorForcedOpen(this.parent.GetRoom(RegionType.Set_All)))
			{
				string str3 = text;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler3 = new DefaultInterpolatedStringHandler(3, 1);
				defaultInterpolatedStringHandler3.AppendLiteral(" (");
				defaultInterpolatedStringHandler3.AppendFormatted<TaggedString>("Stat_ContainmentStrength_DoorForcedOpen".Translate());
				defaultInterpolatedStringHandler3.AppendLiteral(")");
				text = str3 + defaultInterpolatedStringHandler3.ToStringAndClear();
			}
			return text;
		}
	}
}
