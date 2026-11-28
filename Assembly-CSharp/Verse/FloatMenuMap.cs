using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020005F3 RID: 1523
	public class FloatMenuMap : FloatMenu
	{
		// Token: 0x06002FDB RID: 12251 RVA: 0x0013281F File Offset: 0x00130A1F
		public FloatMenuMap(List<FloatMenuOption> options, string title, Vector3 clickPos) : base(options, title, false)
		{
			this.clickPos = clickPos;
		}

		// Token: 0x06002FDC RID: 12252 RVA: 0x00132834 File Offset: 0x00130A34
		public override void DoWindowContents(Rect inRect)
		{
			if (!Find.Selector.AnyPawnSelected)
			{
				Find.WindowStack.TryRemove(this, true);
				return;
			}
			bool flag = this.options.Count >= 3;
			if (Time.frameCount % 4 == 0 || this.lastOptionsForRevalidation == null)
			{
				FloatMenuContext floatMenuContext;
				this.lastOptionsForRevalidation = FloatMenuMakerMap.GetOptions(Find.Selector.SelectedPawns, this.clickPos, out floatMenuContext);
				FloatMenuMap.cachedChoices.Clear();
				FloatMenuMap.cachedChoices.Add(this.clickPos, this.lastOptionsForRevalidation);
				if (!flag)
				{
					for (int i = 0; i < this.options.Count; i++)
					{
						this.<DoWindowContents>g__RevalidateOption|6_0(this.options[i]);
					}
				}
			}
			else if (flag)
			{
				if (this.nextOptionToRevalidate >= this.options.Count)
				{
					this.nextOptionToRevalidate = 0;
				}
				int num = Mathf.CeilToInt((float)this.options.Count / 3f);
				int num2 = this.nextOptionToRevalidate;
				int num3 = 0;
				while (num2 < this.options.Count && num3 < num)
				{
					this.<DoWindowContents>g__RevalidateOption|6_0(this.options[num2]);
					this.nextOptionToRevalidate++;
					num2++;
					num3++;
				}
			}
			base.DoWindowContents(inRect);
		}

		// Token: 0x06002FDD RID: 12253 RVA: 0x0013297C File Offset: 0x00130B7C
		private static bool StillValid(FloatMenuOption opt, List<FloatMenuOption> curOpts)
		{
			if (opt.revalidateClickTarget == null)
			{
				for (int i = 0; i < curOpts.Count; i++)
				{
					if (FloatMenuMap.OptionsMatch(opt, curOpts[i]))
					{
						return true;
					}
				}
			}
			else
			{
				if (!opt.targetsDespawned && !opt.revalidateClickTarget.Spawned)
				{
					return false;
				}
				Vector3 key = opt.revalidateClickTarget.PositionHeld.ToVector3Shifted();
				List<FloatMenuOption> list;
				if (!FloatMenuMap.cachedChoices.TryGetValue(key, out list))
				{
					FloatMenuContext floatMenuContext;
					List<FloatMenuOption> options = FloatMenuMakerMap.GetOptions(Find.Selector.SelectedPawns, key, out floatMenuContext);
					FloatMenuMap.cachedChoices.Add(key, options);
					list = options;
				}
				for (int j = 0; j < list.Count; j++)
				{
					if (FloatMenuMap.OptionsMatch(opt, list[j]))
					{
						return !list[j].Disabled;
					}
				}
			}
			return false;
		}

		// Token: 0x06002FDE RID: 12254 RVA: 0x00132A50 File Offset: 0x00130C50
		public override void PreOptionChosen(FloatMenuOption opt)
		{
			base.PreOptionChosen(opt);
			FloatMenuContext floatMenuContext;
			if (!opt.Disabled && !FloatMenuMap.StillValid(opt, FloatMenuMakerMap.GetOptions(Find.Selector.SelectedPawns, this.clickPos, out floatMenuContext)))
			{
				opt.Disabled = true;
			}
		}

		// Token: 0x06002FDF RID: 12255 RVA: 0x00132A92 File Offset: 0x00130C92
		private static bool OptionsMatch(FloatMenuOption a, FloatMenuOption b)
		{
			return a.Label == b.Label;
		}

		// Token: 0x06002FE1 RID: 12257 RVA: 0x00132AB6 File Offset: 0x00130CB6
		[CompilerGenerated]
		private void <DoWindowContents>g__RevalidateOption|6_0(FloatMenuOption option)
		{
			if (!option.Disabled && !FloatMenuMap.StillValid(option, this.lastOptionsForRevalidation))
			{
				option.Disabled = true;
			}
		}

		// Token: 0x04001E76 RID: 7798
		private Vector3 clickPos;

		// Token: 0x04001E77 RID: 7799
		private static Dictionary<Vector3, List<FloatMenuOption>> cachedChoices = new Dictionary<Vector3, List<FloatMenuOption>>();

		// Token: 0x04001E78 RID: 7800
		private List<FloatMenuOption> lastOptionsForRevalidation;

		// Token: 0x04001E79 RID: 7801
		private int nextOptionToRevalidate;

		// Token: 0x04001E7A RID: 7802
		public const int RevalidateEveryFrame = 4;
	}
}
