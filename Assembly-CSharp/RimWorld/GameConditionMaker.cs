using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001130 RID: 4400
	public static class GameConditionMaker
	{
		// Token: 0x06006E70 RID: 28272 RVA: 0x0026BEB7 File Offset: 0x0026A0B7
		public static GameCondition MakeConditionPermanent(GameConditionDef def)
		{
			GameCondition gameCondition = GameConditionMaker.MakeCondition(def, -1);
			gameCondition.Permanent = true;
			gameCondition.startTick -= 180000;
			return gameCondition;
		}

		// Token: 0x06006E71 RID: 28273 RVA: 0x0026BEDC File Offset: 0x0026A0DC
		public static GameCondition MakeCondition(GameConditionDef def, int duration = -1)
		{
			GameCondition gameCondition = (GameCondition)Activator.CreateInstance(def.conditionClass);
			gameCondition.startTick = Find.TickManager.TicksGame;
			gameCondition.def = def;
			gameCondition.Duration = duration;
			gameCondition.uniqueID = Find.UniqueIDsManager.GetNextGameConditionID();
			gameCondition.PostMake();
			return gameCondition;
		}
	}
}
