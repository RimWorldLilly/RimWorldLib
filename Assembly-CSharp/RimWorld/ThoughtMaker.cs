using System;

namespace RimWorld
{
	// Token: 0x020015D0 RID: 5584
	public static class ThoughtMaker
	{
		// Token: 0x06008ABC RID: 35516 RVA: 0x003256E6 File Offset: 0x003238E6
		public static Thought MakeThought(ThoughtDef def)
		{
			Thought thought = (Thought)Activator.CreateInstance(def.ThoughtClass);
			thought.def = def;
			thought.Init();
			return thought;
		}

		// Token: 0x06008ABD RID: 35517 RVA: 0x00325705 File Offset: 0x00323905
		public static Thought_Memory MakeThought(ThoughtDef def, int forcedStage)
		{
			Thought_Memory thought_Memory = (Thought_Memory)Activator.CreateInstance(def.ThoughtClass);
			thought_Memory.def = def;
			thought_Memory.SetForcedStage(forcedStage);
			thought_Memory.Init();
			return thought_Memory;
		}

		// Token: 0x06008ABE RID: 35518 RVA: 0x0032572B File Offset: 0x0032392B
		public static Thought_Memory MakeThought(ThoughtDef def, Precept sourcePrecept)
		{
			Thought_Memory thought_Memory = (Thought_Memory)Activator.CreateInstance(def.ThoughtClass);
			thought_Memory.def = def;
			thought_Memory.sourcePrecept = sourcePrecept;
			thought_Memory.Init();
			return thought_Memory;
		}
	}
}
