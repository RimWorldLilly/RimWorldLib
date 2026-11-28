using System;
using System.Runtime.CompilerServices;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001462 RID: 5218
	public class CompAbilityEffect_Transmute : CompAbilityEffect
	{
		// Token: 0x170015F3 RID: 5619
		// (get) Token: 0x06008027 RID: 32807 RVA: 0x002DB39B File Offset: 0x002D959B
		public new CompProperties_Transmute Props
		{
			get
			{
				return (CompProperties_Transmute)this.props;
			}
		}

		// Token: 0x06008028 RID: 32808 RVA: 0x002DB3A8 File Offset: 0x002D95A8
		public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
		{
			base.Apply(target, dest);
			Thing thing = target.Thing;
			if (thing.Stuff != null)
			{
				string value = Find.ActiveLanguageWorker.WithIndefiniteArticle(thing.LabelShort, false, false);
				string value2 = Find.ActiveLanguageWorker.WithIndefiniteArticle(Find.ActiveLanguageWorker.Pluralize(thing.LabelNoCount, -1), false, false);
				ThingDef stuff = thing.Stuff;
				ThingDef thingDef;
				using (new RandBlock(thing.thingIDNumber))
				{
					thingDef = this.Props.outcomeStuff.RandomElement<ThingDef>();
				}
				float num = (float)thing.HitPoints / (float)thing.MaxHitPoints;
				thing.SetStuffDirect(thingDef);
				StatDefOf.MaxHitPoints.Worker.ClearCacheForThing(thing);
				thing.Notify_ColorChanged();
				thing.DirtyMapMesh(thing.Map);
				thing.HitPoints = Mathf.CeilToInt((float)thing.MaxHitPoints * num);
				UnfinishedThing unfinishedThing = thing as UnfinishedThing;
				if (unfinishedThing != null)
				{
					int num2 = 0;
					int num3 = 100;
					foreach (Thing thing2 in unfinishedThing.ingredients)
					{
						num2 += thing2.stackCount;
					}
					unfinishedThing.ingredients.Clear();
					while (num2 > 0 && num3-- > 0)
					{
						Thing thing3 = ThingMaker.MakeThing(thingDef, null);
						thing3.stackCount = Mathf.Min(num2, thingDef.stackLimit);
						unfinishedThing.ingredients.Add(thing3);
						num2 -= thing3.stackCount;
					}
					if (num3 <= 0)
					{
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(122, 3);
						defaultInterpolatedStringHandler.AppendLiteral("Attempted to transmute an unfinished thing but could not generate enough replacement items (");
						defaultInterpolatedStringHandler.AppendFormatted(stuff.label);
						defaultInterpolatedStringHandler.AppendLiteral(" to ");
						defaultInterpolatedStringHandler.AppendFormatted(thingDef.label);
						defaultInterpolatedStringHandler.AppendLiteral("). Check if ");
						defaultInterpolatedStringHandler.AppendFormatted(thingDef.label);
						defaultInterpolatedStringHandler.AppendLiteral(" is stackable.");
						Debug.LogError(defaultInterpolatedStringHandler.ToStringAndClear());
					}
				}
				string text;
				if (target.Thing.stackCount > 1)
				{
					text = "MessageTransmutedStuffPlural".Translate(this.parent.pawn.Named("PAWN"), value2, thing.Named("TRANSMUTED"));
				}
				else
				{
					text = "MessageTransmutedStuff".Translate(this.parent.pawn.Named("PAWN"), value, thing.Named("TRANSMUTED"));
				}
				Messages.Message(text, this.parent.pawn, MessageTypeDefOf.NeutralEvent, true);
				return;
			}
			CompProperties_Transmute.ElementRatio elementRatio;
			this.TryGetElementFor(thing.def, out elementRatio);
			int stackCount = thing.stackCount;
			int num4 = Mathf.Max(Mathf.FloorToInt((float)thing.stackCount * elementRatio.ratio), 1);
			Thing thing4 = ThingMaker.MakeThing(this.Props.outcomeItems.RandomElement<ThingDef>(), null);
			thing4.stackCount = num4;
			IntVec3 positionHeld = thing.PositionHeld;
			thing.Destroy(DestroyMode.Vanish);
			GenPlace.TryPlaceThing(thing4, positionHeld, this.parent.pawn.Map, ThingPlaceMode.Direct, null, null, null, 1);
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2 = new DefaultInterpolatedStringHandler(1, 2);
			defaultInterpolatedStringHandler2.AppendFormatted<int>(stackCount);
			defaultInterpolatedStringHandler2.AppendLiteral(" ");
			defaultInterpolatedStringHandler2.AppendFormatted(target.Thing.LabelNoCount);
			string arg = defaultInterpolatedStringHandler2.ToStringAndClear();
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler3 = new DefaultInterpolatedStringHandler(1, 2);
			defaultInterpolatedStringHandler3.AppendFormatted<int>(num4);
			defaultInterpolatedStringHandler3.AppendLiteral(" ");
			defaultInterpolatedStringHandler3.AppendFormatted(thing4.LabelNoCount);
			string arg2 = defaultInterpolatedStringHandler3.ToStringAndClear();
			Messages.Message("MessageTransmutedItem".Translate(this.parent.pawn.Named("PAWN"), arg.Named("ORIGINAL"), arg2.Named("TRANSMUTED")), this.parent.pawn, MessageTypeDefOf.NeutralEvent, true);
		}

		// Token: 0x06008029 RID: 32809 RVA: 0x002DB7B8 File Offset: 0x002D99B8
		public override bool CanApplyOn(GlobalTargetInfo target)
		{
			CompProperties_Transmute.ElementRatio elementRatio;
			return target.HasThing && this.TryGetElementFor(target.Thing.Stuff ?? target.Thing.def, out elementRatio);
		}

		// Token: 0x0600802A RID: 32810 RVA: 0x002DB7F4 File Offset: 0x002D99F4
		public override bool CanApplyOn(LocalTargetInfo target, LocalTargetInfo dest)
		{
			CompProperties_Transmute.ElementRatio elementRatio;
			bool flag = target.HasThing && this.TryGetElementFor(target.Thing.Stuff ?? target.Thing.def, out elementRatio);
			if (!flag)
			{
				Messages.Message(this.Props.failedMessage.Formatted(target.Thing.Label), target.Thing, MessageTypeDefOf.NeutralEvent, true);
			}
			return flag;
		}

		// Token: 0x0600802B RID: 32811 RVA: 0x002DB874 File Offset: 0x002D9A74
		private bool TryGetElementFor(ThingDef stuff, out CompProperties_Transmute.ElementRatio ratio)
		{
			ratio = this.Props.elementRatios.FirstOrDefault((CompProperties_Transmute.ElementRatio x) => x.sourceStuff == stuff);
			return ratio != null;
		}

		// Token: 0x0600802C RID: 32812 RVA: 0x002DB8B1 File Offset: 0x002D9AB1
		public override bool AICanTargetNow(LocalTargetInfo target)
		{
			return false;
		}
	}
}
