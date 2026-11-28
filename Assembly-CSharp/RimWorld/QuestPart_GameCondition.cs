using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x0200111B RID: 4379
	public class QuestPart_GameCondition : QuestPart
	{
		// Token: 0x170012C8 RID: 4808
		// (get) Token: 0x06006D7F RID: 28031 RVA: 0x00265CAC File Offset: 0x00263EAC
		public override IEnumerable<GlobalTargetInfo> QuestLookTargets
		{
			get
			{
				QuestPart_GameCondition.<get_QuestLookTargets>d__6 <get_QuestLookTargets>d__ = new QuestPart_GameCondition.<get_QuestLookTargets>d__6(-2);
				<get_QuestLookTargets>d__.<>4__this = this;
				return <get_QuestLookTargets>d__;
			}
		}

		// Token: 0x06006D80 RID: 28032 RVA: 0x00265CBC File Offset: 0x00263EBC
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag == this.inSignal && (this.targetWorld || this.mapParent != null) && this.gameCondition != null)
			{
				this.gameCondition.quest = this.quest;
				if (this.targetWorld)
				{
					Find.World.gameConditionManager.RegisterCondition(this.gameCondition);
				}
				else
				{
					Map map;
					if ((map = this.mapParent.Map) == null)
					{
						MapParent mapParent = this.quest.TryFindNewSuitableMapParentForRetarget();
						map = (((mapParent != null) ? mapParent.Map : null) ?? Find.AnyPlayerHomeMap);
					}
					Map map2 = map;
					if (!this.gameCondition.CanApplyOnMap(map2))
					{
						bool flag = false;
						foreach (Map map3 in Current.Game.PlayerHomeMaps)
						{
							if (this.gameCondition.CanApplyOnMap(map3))
							{
								map2 = map3;
								flag = true;
								break;
							}
						}
						if (!flag)
						{
							return;
						}
					}
					map2.gameConditionManager.RegisterCondition(this.gameCondition);
				}
				if (this.sendStandardLetter)
				{
					Find.LetterStack.ReceiveLetter(this.gameCondition.LabelCap, this.gameCondition.LetterText, this.gameCondition.def.letterDef, LookTargets.Invalid, null, this.quest, null, null, 0, true);
				}
				this.gameCondition = null;
			}
		}

		// Token: 0x06006D81 RID: 28033 RVA: 0x00265E38 File Offset: 0x00264038
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_Deep.Look<GameCondition>(ref this.gameCondition, "gameCondition", Array.Empty<object>());
			Scribe_Values.Look<bool>(ref this.targetWorld, "targetWorld", false, false);
			Scribe_References.Look<MapParent>(ref this.mapParent, "mapParent", false);
			Scribe_Values.Look<bool>(ref this.sendStandardLetter, "sendStandardLetter", true, false);
		}

		// Token: 0x06006D82 RID: 28034 RVA: 0x00265EA8 File Offset: 0x002640A8
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.inSignal = "DebugSignal" + Rand.Int.ToString();
			if (Find.AnyPlayerHomeMap != null)
			{
				this.gameCondition = GameConditionMaker.MakeCondition(GameConditionDefOf.ColdSnap, Rand.RangeInclusive(2500, 7500));
				this.mapParent = Find.RandomPlayerHomeMap.Parent;
			}
		}

		// Token: 0x040045C1 RID: 17857
		public string inSignal;

		// Token: 0x040045C2 RID: 17858
		public GameCondition gameCondition;

		// Token: 0x040045C3 RID: 17859
		public bool targetWorld;

		// Token: 0x040045C4 RID: 17860
		public MapParent mapParent;

		// Token: 0x040045C5 RID: 17861
		public bool sendStandardLetter = true;
	}
}
