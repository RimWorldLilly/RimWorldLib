using System;
using System.Linq;
using RimWorld.QuestGen;
using Verse;

namespace RimWorld
{
	// Token: 0x02001100 RID: 4352
	public class QuestPart_SubquestGenerator_ArchonexusVictory : QuestPart_SubquestGenerator
	{
		// Token: 0x06006C9D RID: 27805 RVA: 0x002618AB File Offset: 0x0025FAAB
		protected override Slate InitSlate()
		{
			Slate slate = new Slate();
			slate.Set<Faction>("civilOutlander", this.civilOutlander, false);
			slate.Set<Faction>("roughTribe", this.roughTribe, false);
			slate.Set<Faction>("roughOutlander", this.roughOutlander, false);
			return slate;
		}

		// Token: 0x06006C9E RID: 27806 RVA: 0x002618E8 File Offset: 0x0025FAE8
		protected override QuestScriptDef GetNextSubquestDef()
		{
			int index = this.quest.GetSubquests(new QuestState?(QuestState.EndedSuccess)).Count<Quest>() % this.subquestDefs.Count;
			QuestScriptDef questScriptDef = this.subquestDefs[index];
			if (!questScriptDef.CanRun(this.InitSlate(), Find.World))
			{
				return null;
			}
			return questScriptDef;
		}

		// Token: 0x06006C9F RID: 27807 RVA: 0x0026193B File Offset: 0x0025FB3B
		public override void Notify_FactionRemoved(Faction faction)
		{
			if (this.civilOutlander == faction)
			{
				this.civilOutlander = null;
			}
			if (this.roughTribe == faction)
			{
				this.roughTribe = null;
			}
			if (this.roughOutlander == faction)
			{
				this.roughOutlander = null;
			}
		}

		// Token: 0x06006CA0 RID: 27808 RVA: 0x0026196D File Offset: 0x0025FB6D
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Faction>(ref this.civilOutlander, "civilOutlander", false);
			Scribe_References.Look<Faction>(ref this.roughTribe, "roughTribe", false);
			Scribe_References.Look<Faction>(ref this.roughOutlander, "roughOutlander", false);
		}

		// Token: 0x04004523 RID: 17699
		public Faction civilOutlander;

		// Token: 0x04004524 RID: 17700
		public Faction roughTribe;

		// Token: 0x04004525 RID: 17701
		public Faction roughOutlander;
	}
}
