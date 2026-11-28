using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x020010C8 RID: 4296
	public class QuestPart_InspectString : QuestPartActivable
	{
		// Token: 0x17001261 RID: 4705
		// (get) Token: 0x06006B64 RID: 27492 RVA: 0x0025A95A File Offset: 0x00258B5A
		public override IEnumerable<GlobalTargetInfo> QuestLookTargets
		{
			get
			{
				QuestPart_InspectString.<get_QuestLookTargets>d__5 <get_QuestLookTargets>d__ = new QuestPart_InspectString.<get_QuestLookTargets>d__5(-2);
				<get_QuestLookTargets>d__.<>4__this = this;
				return <get_QuestLookTargets>d__;
			}
		}

		// Token: 0x06006B65 RID: 27493 RVA: 0x0025A96A File Offset: 0x00258B6A
		protected override void Enable(SignalArgs receivedArgs)
		{
			base.Enable(receivedArgs);
			this.resolvedInspectString = receivedArgs.GetFormattedText(this.inspectString);
		}

		// Token: 0x06006B66 RID: 27494 RVA: 0x0025A990 File Offset: 0x00258B90
		public override string ExtraInspectString(ISelectable target)
		{
			if (this.targets.Contains(target))
			{
				return this.resolvedInspectString;
			}
			return null;
		}

		// Token: 0x06006B67 RID: 27495 RVA: 0x0025A9A8 File Offset: 0x00258BA8
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<ISelectable>(ref this.targets, "targets", LookMode.Reference, Array.Empty<object>());
			Scribe_Values.Look<string>(ref this.inspectString, "inspectString", null, false);
			Scribe_Values.Look<string>(ref this.resolvedInspectString, "resolvedInspectString", null, false);
		}

		// Token: 0x06006B68 RID: 27496 RVA: 0x0025A9F5 File Offset: 0x00258BF5
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			if (Find.AnyPlayerHomeMap != null)
			{
				this.targets.Add(Find.RandomPlayerHomeMap.mapPawns.FreeColonists.FirstOrDefault<Pawn>());
				this.inspectString = "Debug inspect string.";
			}
		}

		// Token: 0x04004442 RID: 17474
		public List<ISelectable> targets = new List<ISelectable>();

		// Token: 0x04004443 RID: 17475
		public string inspectString;

		// Token: 0x04004444 RID: 17476
		private string resolvedInspectString;

		// Token: 0x04004445 RID: 17477
		private ILoadReferenceable targetRef;
	}
}
