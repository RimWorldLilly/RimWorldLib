using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse.AI;

namespace Verse
{
	// Token: 0x0200031E RID: 798
	public sealed class TooltipGiverList
	{
		// Token: 0x0600187F RID: 6271 RVA: 0x00093234 File Offset: 0x00091434
		public void Notify_ThingSpawned(Thing t)
		{
			if (t.def.hasTooltip || this.ShouldShowShotReport(t))
			{
				this.givers.Add(t);
			}
		}

		// Token: 0x06001880 RID: 6272 RVA: 0x00093258 File Offset: 0x00091458
		public void Notify_ThingDespawned(Thing t)
		{
			if (t.def.hasTooltip || this.ShouldShowShotReport(t))
			{
				this.givers.Remove(t);
			}
		}

		// Token: 0x06001881 RID: 6273 RVA: 0x00093280 File Offset: 0x00091480
		public void DispenseAllThingTooltips()
		{
			if (Event.current.type != EventType.repaint)
			{
				return;
			}
			if (Find.WindowStack.FloatMenu != null)
			{
				return;
			}
			if (Find.Targeter.IsTargeting && Find.Targeter.targetingSource != null && Find.Targeter.targetingSource.HidePawnTooltips)
			{
				return;
			}
			CellRect currentViewRect = Find.CameraDriver.CurrentViewRect;
			float cellSizePixels = Find.CameraDriver.CellSizePixels;
			Vector2 vector = new Vector2(cellSizePixels, cellSizePixels);
			Rect rect = new Rect(0f, 0f, vector.x, vector.y);
			for (int i = 0; i < this.givers.Count; i++)
			{
				Thing thing = this.givers[i];
				if (currentViewRect.Contains(thing.Position) && !thing.Position.Fogged(thing.Map))
				{
					Vector2 vector2 = thing.DrawPos.MapToUIPosition();
					rect.x = vector2.x - vector.x / 2f;
					rect.y = vector2.y - vector.y / 2f;
					if (rect.Contains(Event.current.mousePosition))
					{
						string text = this.ShouldShowShotReport(thing) ? TooltipUtility.ShotCalculationTipString(thing) : null;
						if (thing.def.hasTooltip || !text.NullOrEmpty())
						{
							Pawn pawn = thing as Pawn;
							if (pawn != null && pawn.IsHiddenFromPlayer())
							{
								return;
							}
							TipSignal tooltip = thing.GetTooltip();
							if (!text.NullOrEmpty())
							{
								tooltip.text = tooltip.text + "\n\n" + text;
							}
							TooltipHandler.TipRegion(rect, tooltip);
						}
					}
				}
			}
		}

		// Token: 0x06001882 RID: 6274 RVA: 0x00093433 File Offset: 0x00091633
		private bool ShouldShowShotReport(Thing t)
		{
			return t.def.hasTooltip || t is Hive || t is IAttackTarget;
		}

		// Token: 0x04001468 RID: 5224
		private List<Thing> givers = new List<Thing>();
	}
}
