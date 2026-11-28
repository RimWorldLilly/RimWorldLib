using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001CFF RID: 7423
	public class Page_ConfigureFluidIdeo : Page_ConfigureIdeo
	{
		// Token: 0x0600BFA1 RID: 49057 RVA: 0x004599B9 File Offset: 0x00457BB9
		public override void PostOpen()
		{
			FactionIdeosTracker ideos = Faction.OfPlayer.ideos;
			IdeoUIUtility.selected = ((ideos != null) ? ideos.PrimaryIdeo : null);
			base.PostOpen();
		}

		// Token: 0x0600BFA2 RID: 49058 RVA: 0x004599DC File Offset: 0x00457BDC
		public override void DoIdeos(Rect rect)
		{
			IdeoUIUtility.DoIdeoListAndDetails(base.GetMainRect(rect, 0f, false), ref this.scrollPosition_ideoList, ref this.scrollViewHeight_ideoList, ref this.scrollPosition_ideoDetails, ref this.scrollViewHeight_ideoDetails, true, false, null, null, null, false, null, new Action<Ideo>(this.<DoIdeos>g__loadFluidOrNpcIdeo|1_0), false, true, null);
		}

		// Token: 0x0600BFA3 RID: 49059 RVA: 0x00459A29 File Offset: 0x00457C29
		private void SetLoadedIdeo(Ideo loadedIdeo)
		{
			this.ideo = loadedIdeo;
			IdeoUIUtility.MakeLoadedIdeoPrimary(loadedIdeo);
			this.ideo.Fluid = true;
		}

		// Token: 0x0600BFA4 RID: 49060 RVA: 0x00459A44 File Offset: 0x00457C44
		private bool TryFindIncompatibleMemes(Ideo ideo, out List<MemeDef> memes)
		{
			memes = null;
			if (ideo.memes.Count((MemeDef m) => m.category == MemeCategory.Normal) > IdeoFoundation.MemeCountRangeFluidAbsolute.max)
			{
				return true;
			}
			for (int i = 0; i < ideo.memes.Count; i++)
			{
				if (!IdeoUtility.IsMemeAllowedForInitialFluidIdeo(ideo.memes[i]))
				{
					if (memes == null)
					{
						memes = new List<MemeDef>();
					}
					memes.Add(ideo.memes[i]);
				}
			}
			return memes != null && memes.Count > 0;
		}

		// Token: 0x0600BFA5 RID: 49061 RVA: 0x00459AE4 File Offset: 0x00457CE4
		public override void Notify_ClosedChooseMemesDialog()
		{
			if (this.ideo != null)
			{
				if (!this.ideo.memes.Any((MemeDef x) => x.category == MemeCategory.Normal) && this.prev != null)
				{
					this.DoBack();
				}
			}
		}

		// Token: 0x0600BFA7 RID: 49063 RVA: 0x00459B40 File Offset: 0x00457D40
		[CompilerGenerated]
		private void <DoIdeos>g__loadFluidOrNpcIdeo|1_0(Ideo loadedIdeo)
		{
			List<Faction> factionsWithIdeo = Find.IdeoManager.GetFactionsWithIdeo(IdeoUIUtility.selected, true, true);
			if (factionsWithIdeo.Count != 0)
			{
				if (!Find.IdeoManager.IdeosListForReading.Contains(loadedIdeo))
				{
					Find.IdeoManager.Add(loadedIdeo);
				}
				IdeoUIUtility.SetSelected(loadedIdeo);
				foreach (Faction faction in factionsWithIdeo)
				{
					faction.ideos.SetPrimary(loadedIdeo);
				}
				Find.IdeoManager.RemoveUnusedStartingIdeos();
				return;
			}
			loadedIdeo.Fluid = true;
			List<MemeDef> list;
			if (this.TryFindIncompatibleMemes(loadedIdeo, out list))
			{
				TaggedString taggedString = "ConfirmChangeMemesForFluidIdeo".Translate();
				if (!list.NullOrEmpty<MemeDef>())
				{
					string t = (from m in list
					select m.LabelCap.Resolve()).ToLineList("- ", false);
					taggedString += " " + "ConfirmIncompatibleMemes".Translate() + ":\n\n" + t;
				}
				taggedString += "\n\n" + "ConfirmChangeMemesForFluidIdeoContinue".Translate();
				Action <>9__4;
				Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation(taggedString, delegate()
				{
					loadedIdeo.memes.RemoveAll((MemeDef m) => m.category == MemeCategory.Normal);
					loadedIdeo.ClearPrecepts();
					WindowStack windowStack = Find.WindowStack;
					Ideo loadedIdeo2 = loadedIdeo;
					MemeCategory memeCategory = MemeCategory.Normal;
					bool initialSelection = true;
					Action done;
					if ((done = <>9__4) == null)
					{
						done = (<>9__4 = delegate()
						{
							this.SetLoadedIdeo(loadedIdeo);
						});
					}
					windowStack.Add(new Dialog_ChooseMemes(loadedIdeo2, memeCategory, initialSelection, done, null, false));
				}, false, null, WindowLayer.Dialog));
				return;
			}
			this.SetLoadedIdeo(loadedIdeo);
		}
	}
}
