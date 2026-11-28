using System;
using System.Collections.Generic;
using RimWorld.Planet;
using UnityEngine;

namespace Verse
{
	// Token: 0x020005F6 RID: 1526
	public class FloatMenuWorld : FloatMenu
	{
		// Token: 0x06002FFB RID: 12283 RVA: 0x00133504 File Offset: 0x00131704
		public FloatMenuWorld(List<FloatMenuOption> options, string title, Vector2 clickPos) : base(options, title, false)
		{
			this.clickPos = clickPos;
		}

		// Token: 0x06002FFC RID: 12284 RVA: 0x00133518 File Offset: 0x00131718
		public override void DoWindowContents(Rect inRect)
		{
			Caravan caravan = Find.WorldSelector.SingleSelectedObject as Caravan;
			if (caravan == null)
			{
				Find.WindowStack.TryRemove(this, true);
				return;
			}
			if (Time.frameCount % 4 == 0)
			{
				List<FloatMenuOption> list = FloatMenuMakerWorld.ChoicesAtFor(this.clickPos, caravan);
				List<FloatMenuOption> list2 = list;
				Vector2 vector = this.clickPos;
				for (int i = 0; i < this.options.Count; i++)
				{
					if (!this.options[i].Disabled && !FloatMenuWorld.StillValid(this.options[i], list, caravan, ref list2, ref vector))
					{
						this.options[i].Disabled = true;
					}
				}
			}
			base.DoWindowContents(inRect);
		}

		// Token: 0x06002FFD RID: 12285 RVA: 0x001335C8 File Offset: 0x001317C8
		private static bool StillValid(FloatMenuOption opt, List<FloatMenuOption> curOpts, Caravan forCaravan)
		{
			List<FloatMenuOption> list = null;
			Vector2 vector = new Vector2(-9999f, -9999f);
			return FloatMenuWorld.StillValid(opt, curOpts, forCaravan, ref list, ref vector);
		}

		// Token: 0x06002FFE RID: 12286 RVA: 0x001335F4 File Offset: 0x001317F4
		private static bool StillValid(FloatMenuOption opt, List<FloatMenuOption> curOpts, Caravan forCaravan, ref List<FloatMenuOption> cachedChoices, ref Vector2 cachedChoicesForPos)
		{
			if (opt.revalidateWorldClickTarget == null)
			{
				for (int i = 0; i < curOpts.Count; i++)
				{
					if (FloatMenuWorld.OptionsMatch(opt, curOpts[i]))
					{
						return true;
					}
				}
			}
			else
			{
				if (!opt.revalidateWorldClickTarget.Spawned)
				{
					return false;
				}
				Vector2 vector = opt.revalidateWorldClickTarget.ScreenPos();
				vector.y = (float)UI.screenHeight - vector.y;
				List<FloatMenuOption> list;
				if (vector == cachedChoicesForPos)
				{
					list = cachedChoices;
				}
				else
				{
					cachedChoices = FloatMenuMakerWorld.ChoicesAtFor(vector, forCaravan);
					cachedChoicesForPos = vector;
					list = cachedChoices;
				}
				for (int j = 0; j < list.Count; j++)
				{
					if (FloatMenuWorld.OptionsMatch(opt, list[j]))
					{
						return !list[j].Disabled;
					}
				}
			}
			return false;
		}

		// Token: 0x06002FFF RID: 12287 RVA: 0x001336B8 File Offset: 0x001318B8
		public override void PreOptionChosen(FloatMenuOption opt)
		{
			base.PreOptionChosen(opt);
			Caravan caravan = Find.WorldSelector.SingleSelectedObject as Caravan;
			if (!opt.Disabled && (caravan == null || !FloatMenuWorld.StillValid(opt, FloatMenuMakerWorld.ChoicesAtFor(this.clickPos, caravan), caravan)))
			{
				opt.Disabled = true;
			}
		}

		// Token: 0x06003000 RID: 12288 RVA: 0x00133703 File Offset: 0x00131903
		private static bool OptionsMatch(FloatMenuOption a, FloatMenuOption b)
		{
			return a.Label == b.Label;
		}

		// Token: 0x04001EB3 RID: 7859
		private Vector2 clickPos;

		// Token: 0x04001EB4 RID: 7860
		private const int RevalidateEveryFrame = 4;
	}
}
