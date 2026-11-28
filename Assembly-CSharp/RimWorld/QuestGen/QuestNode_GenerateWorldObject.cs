using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x020021B4 RID: 8628
	public class QuestNode_GenerateWorldObject : QuestNode
	{
		// Token: 0x0600D6F6 RID: 55030 RVA: 0x004FCBC8 File Offset: 0x004FADC8
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			WorldObject worldObject = WorldObjectMaker.MakeWorldObject(this.def.GetValue(slate));
			worldObject.Tile = this.tile.GetValue(slate);
			if (this.faction.GetValue(slate) != null)
			{
				worldObject.SetFaction(this.faction.GetValue(slate));
			}
			if (this.storeAs.GetValue(slate) != null)
			{
				QuestGen.slate.Set<WorldObject>(this.storeAs.GetValue(slate), worldObject, false);
			}
		}

		// Token: 0x0600D6F7 RID: 55031 RVA: 0x004FCC45 File Offset: 0x004FAE45
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x04008856 RID: 34902
		public SlateRef<WorldObjectDef> def;

		// Token: 0x04008857 RID: 34903
		public SlateRef<PlanetTile> tile;

		// Token: 0x04008858 RID: 34904
		public SlateRef<Faction> faction;

		// Token: 0x04008859 RID: 34905
		[NoTranslate]
		public SlateRef<string> storeAs;
	}
}
