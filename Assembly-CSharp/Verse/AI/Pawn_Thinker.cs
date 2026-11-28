using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x020008B3 RID: 2227
	public class Pawn_Thinker : IExposable
	{
		// Token: 0x17000D01 RID: 3329
		// (get) Token: 0x06004547 RID: 17735 RVA: 0x001B4FAC File Offset: 0x001B31AC
		public ThinkTreeDef MainThinkTree
		{
			get
			{
				Pawn_MutantTracker mutant = this.pawn.mutant;
				ThinkTreeDef result;
				if ((result = ((mutant != null) ? mutant.GetThinkTrees().main : null)) == null)
				{
					LifeStageDef curLifeStage = this.pawn.ageTracker.CurLifeStage;
					result = (((curLifeStage != null) ? curLifeStage.thinkTreeMainOverride : null) ?? this.pawn.RaceProps.thinkTreeMain);
				}
				return result;
			}
		}

		// Token: 0x17000D02 RID: 3330
		// (get) Token: 0x06004548 RID: 17736 RVA: 0x001B5009 File Offset: 0x001B3209
		public ThinkNode MainThinkNodeRoot
		{
			get
			{
				return this.MainThinkTree.thinkRoot;
			}
		}

		// Token: 0x17000D03 RID: 3331
		// (get) Token: 0x06004549 RID: 17737 RVA: 0x001B5018 File Offset: 0x001B3218
		public ThinkTreeDef ConstantThinkTree
		{
			get
			{
				Pawn_MutantTracker mutant = this.pawn.mutant;
				ThinkTreeDef result;
				if ((result = ((mutant != null) ? mutant.GetThinkTrees().constant : null)) == null)
				{
					LifeStageDef curLifeStage = this.pawn.ageTracker.CurLifeStage;
					result = (((curLifeStage != null) ? curLifeStage.thinkTreeConstantOverride : null) ?? this.pawn.RaceProps.thinkTreeConstant);
				}
				return result;
			}
		}

		// Token: 0x17000D04 RID: 3332
		// (get) Token: 0x0600454A RID: 17738 RVA: 0x001B5075 File Offset: 0x001B3275
		public ThinkNode ConstantThinkNodeRoot
		{
			get
			{
				return this.ConstantThinkTree.thinkRoot;
			}
		}

		// Token: 0x0600454B RID: 17739 RVA: 0x001B5082 File Offset: 0x001B3282
		public Pawn_Thinker(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x0600454C RID: 17740 RVA: 0x001B5091 File Offset: 0x001B3291
		public T TryGetMainTreeThinkNode<T>() where T : ThinkNode
		{
			return this.MainThinkNodeRoot.FirstNodeOfType<T>(false);
		}

		// Token: 0x0600454D RID: 17741 RVA: 0x001B50A0 File Offset: 0x001B32A0
		public T GetMainTreeThinkNode<T>() where T : ThinkNode
		{
			T t = this.TryGetMainTreeThinkNode<T>();
			if (t == null)
			{
				Pawn pawn = this.pawn;
				string str = (pawn != null) ? pawn.ToString() : null;
				string str2 = " looked for ThinkNode of type ";
				Type typeFromHandle = typeof(T);
				Log.Warning(str + str2 + ((typeFromHandle != null) ? typeFromHandle.ToString() : null) + " and didn't find it.");
			}
			return t;
		}

		// Token: 0x0600454E RID: 17742 RVA: 0x001B50F7 File Offset: 0x001B32F7
		public void ExposeData()
		{
		}

		// Token: 0x04002D44 RID: 11588
		public Pawn pawn;
	}
}
