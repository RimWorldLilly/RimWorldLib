using System;
using UnityEngine;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x02000F7D RID: 3965
	public class InteractionDef : Def
	{
		// Token: 0x1700115C RID: 4444
		// (get) Token: 0x06006586 RID: 25990 RVA: 0x00242BD3 File Offset: 0x00240DD3
		public InteractionWorker Worker
		{
			get
			{
				if (this.workerInt == null)
				{
					this.workerInt = (InteractionWorker)Activator.CreateInstance(this.workerClass);
					this.workerInt.interaction = this;
				}
				return this.workerInt;
			}
		}

		// Token: 0x1700115D RID: 4445
		// (get) Token: 0x06006587 RID: 25991 RVA: 0x00242C05 File Offset: 0x00240E05
		private Texture2D Symbol
		{
			get
			{
				if (this.symbolTex == null)
				{
					this.symbolTex = ContentFinder<Texture2D>.Get(this.symbol, true);
				}
				return this.symbolTex;
			}
		}

		// Token: 0x06006588 RID: 25992 RVA: 0x00242C30 File Offset: 0x00240E30
		public Texture2D GetSymbol(Faction initiatorFaction = null, Ideo initatorIdeo = null)
		{
			InteractionSymbolSource interactionSymbolSource = this.symbolSource;
			if (interactionSymbolSource != InteractionSymbolSource.InitiatorIdeo)
			{
				if (interactionSymbolSource != InteractionSymbolSource.InitiatorFaction)
				{
					return this.Symbol;
				}
				if (initiatorFaction == null)
				{
					return null;
				}
				return initiatorFaction.def.FactionIcon;
			}
			else
			{
				if (Find.IdeoManager.classicMode)
				{
					return this.Symbol;
				}
				if (initatorIdeo == null)
				{
					return null;
				}
				return initatorIdeo.Icon;
			}
		}

		// Token: 0x06006589 RID: 25993 RVA: 0x00242C84 File Offset: 0x00240E84
		public Color? GetSymbolColor(Faction initiatorFaction = null)
		{
			if (initiatorFaction != null && this.symbolSource == InteractionSymbolSource.InitiatorFaction)
			{
				return new Color?(initiatorFaction.Color);
			}
			return null;
		}

		// Token: 0x0600658A RID: 25994 RVA: 0x00242CB2 File Offset: 0x00240EB2
		public override void ResolveReferences()
		{
			base.ResolveReferences();
			if (this.interactionMote == null)
			{
				this.interactionMote = ThingDefOf.Mote_Speech;
			}
		}

		// Token: 0x04003C2B RID: 15403
		private Type workerClass = typeof(InteractionWorker);

		// Token: 0x04003C2C RID: 15404
		public ThingDef interactionMote;

		// Token: 0x04003C2D RID: 15405
		public float socialFightBaseChance;

		// Token: 0x04003C2E RID: 15406
		public ThoughtDef initiatorThought;

		// Token: 0x04003C2F RID: 15407
		public SkillDef initiatorXpGainSkill;

		// Token: 0x04003C30 RID: 15408
		public int initiatorXpGainAmount;

		// Token: 0x04003C31 RID: 15409
		public ThoughtDef recipientThought;

		// Token: 0x04003C32 RID: 15410
		public SkillDef recipientXpGainSkill;

		// Token: 0x04003C33 RID: 15411
		public int recipientXpGainAmount;

		// Token: 0x04003C34 RID: 15412
		public bool ignoreTimeSinceLastInteraction;

		// Token: 0x04003C35 RID: 15413
		[NoTranslate]
		private string symbol;

		// Token: 0x04003C36 RID: 15414
		public InteractionSymbolSource symbolSource;

		// Token: 0x04003C37 RID: 15415
		public RulePack logRulesInitiator;

		// Token: 0x04003C38 RID: 15416
		public RulePack logRulesRecipient;

		// Token: 0x04003C39 RID: 15417
		[Unsaved(false)]
		private InteractionWorker workerInt;

		// Token: 0x04003C3A RID: 15418
		[Unsaved(false)]
		private Texture2D symbolTex;
	}
}
