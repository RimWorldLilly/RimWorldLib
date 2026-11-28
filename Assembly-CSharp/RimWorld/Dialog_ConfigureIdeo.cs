using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001CA0 RID: 7328
	public class Dialog_ConfigureIdeo : Window
	{
		// Token: 0x17002218 RID: 8728
		// (get) Token: 0x0600BB82 RID: 48002 RVA: 0x0042897D File Offset: 0x00426B7D
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(1100f, Mathf.Min(1000f, (float)UI.screenHeight));
			}
		}

		// Token: 0x17002219 RID: 8729
		// (get) Token: 0x0600BB83 RID: 48003 RVA: 0x00428999 File Offset: 0x00426B99
		public Ideo CurrentPrimaryIdeo
		{
			get
			{
				return this.newPrimaryIdeo ?? this.initialPrimaryIdeo;
			}
		}

		// Token: 0x0600BB84 RID: 48004 RVA: 0x004289AC File Offset: 0x00426BAC
		public Dialog_ConfigureIdeo(IEnumerable<Pawn> pawns, Action nextAction, bool forArchonexusRestart = false) : base(null)
		{
			if (!ModLister.CheckIdeology("Configure ideo dialog"))
			{
				return;
			}
			this.forcePause = true;
			this.doCloseX = false;
			this.doCloseButton = false;
			this.closeOnClickedOutside = false;
			this.absorbInputAroundWindow = true;
			this.closeOnCancel = false;
			this.forceCatchAcceptAndCancelEventEvenIfUnfocused = true;
			this.openMenuOnCancel = true;
			this.preventSave = true;
			this.nextAction = nextAction;
			this.forArchonexusRestart = forArchonexusRestart;
			this.pawns.AddRange(pawns);
			this.initialPrimaryIdeo = Faction.OfPlayer.ideos.PrimaryIdeo;
			foreach (Pawn key in pawns)
			{
				this.pawnConvertToIdeo[key] = null;
			}
		}

		// Token: 0x0600BB85 RID: 48005 RVA: 0x00428A94 File Offset: 0x00426C94
		public override void PreOpen()
		{
			base.PreOpen();
			IdeoUIUtility.SetSelected(Faction.OfPlayer.ideos.PrimaryIdeo);
		}

		// Token: 0x0600BB86 RID: 48006 RVA: 0x00428AB0 File Offset: 0x00426CB0
		public override void PostClose()
		{
			base.PostClose();
			IdeoUIUtility.UnselectCurrent();
		}

		// Token: 0x0600BB87 RID: 48007 RVA: 0x00428AC0 File Offset: 0x00426CC0
		public override void DoWindowContents(Rect inRect)
		{
			Text.Font = GameFont.Medium;
			Widgets.Label(new Rect(inRect.x, inRect.y, inRect.width, 45f), "ConfigureIdeoligion".Translate());
			Text.Font = GameFont.Small;
			IdeoUIUtility.DoIdeoListAndDetails(new Rect(inRect.x, inRect.y + 45f, inRect.width, inRect.height - 55f - 45f), ref this.scrollPosition_ideoList, ref this.scrollViewHeight_ideoList, ref this.scrollPosition_ideoDetails, ref this.scrollViewHeight_ideoDetails, this.customOrLoadedIdeo != null, true, this.pawns, this.customOrLoadedIdeo, delegate
			{
				this.CreateNewIdeo(false);
			}, this.forArchonexusRestart, (Pawn p) => this.pawnConvertToIdeo[p] ?? p.Ideo, delegate(Ideo ideo)
			{
				this.CheckRemoveNewIdeoAndMakePrimary(ideo);
				this.customOrLoadedIdeo = ideo;
			}, false, true, delegate
			{
				this.CreateNewIdeo(true);
			});
			if (Widgets.ButtonText(new Rect(inRect.xMax - Window.CloseButSize.x - 18f, inRect.height - 55f, Window.CloseButSize.x, Window.CloseButSize.y), "Next".Translate(), true, true, true, null))
			{
				if (Faction.OfPlayer.ideos.PrimaryIdeo != this.CurrentPrimaryIdeo)
				{
					Faction.OfPlayer.ideos.SetPrimary(this.CurrentPrimaryIdeo);
				}
				foreach (Pawn pawn in this.pawns)
				{
					if (this.pawnConvertToIdeo[pawn] != null)
					{
						pawn.ideo.SetIdeo(this.pawnConvertToIdeo[pawn]);
					}
				}
				this.Close(true);
				this.nextAction();
			}
			IEnumerable<Pawn> source = from p in this.pawns
			where p.IsColonist && p.Ideo != Faction.OfPlayer.ideos.PrimaryIdeo
			select p;
			if (source.Any<Pawn>() && Widgets.ButtonText(new Rect(inRect.center.x - Dialog_ConfigureIdeo.AssignColonistsBtnSize.x / 2f, inRect.height - 55f, Dialog_ConfigureIdeo.AssignColonistsBtnSize.x, Dialog_ConfigureIdeo.AssignColonistsBtnSize.y), "AssignColonists".Translate(), true, true, true, null))
			{
				Find.WindowStack.Add(new Dialog_ChooseColonistsForIdeo(this.CurrentPrimaryIdeo, source, (Pawn p) => p.Ideo != this.initialPrimaryIdeo && p.Ideo != this.CurrentPrimaryIdeo, (Pawn p) => p.Ideo, (Pawn p) => this.pawnConvertToIdeo[p] ?? p.Ideo, delegate(Pawn p, Ideo i)
				{
					if (p.Ideo == i)
					{
						this.pawnConvertToIdeo[p] = null;
						return;
					}
					this.pawnConvertToIdeo[p] = i;
				}));
			}
			Rect rect = new Rect(inRect.x + 18f, inRect.height - 55f, Dialog_ConfigureIdeo.ChangeOrDeleteIdeoBtnSize.x, Dialog_ConfigureIdeo.ChangeOrDeleteIdeoBtnSize.y);
			if (this.customOrLoadedIdeo != null && IdeoUIUtility.selected == this.customOrLoadedIdeo && this.CurrentPrimaryIdeo == this.customOrLoadedIdeo)
			{
				if (Widgets.ButtonText(rect, "RemoveNewIdeoligion".Translate(), true, true, true, null))
				{
					this.CheckRemoveNewIdeoAndMakePrimary(this.initialPrimaryIdeo);
					return;
				}
			}
			else if (IdeoUIUtility.selected != this.CurrentPrimaryIdeo && Widgets.ButtonText(rect, "MakeIdeoligionPrimary".Translate(), true, true, true, null))
			{
				this.CheckRemoveNewIdeoAndMakePrimary(IdeoUIUtility.selected);
			}
		}

		// Token: 0x0600BB88 RID: 48008 RVA: 0x00428E68 File Offset: 0x00427068
		private void CreateNewIdeo(bool fluid = false)
		{
			Ideo newIdeo = IdeoUtility.MakeEmptyIdeo();
			newIdeo.Fluid = fluid;
			Find.WindowStack.Add(new Dialog_ChooseMemes(newIdeo, MemeCategory.Structure, false, delegate()
			{
				this.CheckRemoveNewIdeoAndMakePrimary(newIdeo);
				this.customOrLoadedIdeo = newIdeo;
			}, null, false));
		}

		// Token: 0x0600BB89 RID: 48009 RVA: 0x00428EBE File Offset: 0x004270BE
		private void RemoveNewIdeoIfCreated()
		{
			if (this.customOrLoadedIdeo != null)
			{
				Find.IdeoManager.Remove(this.customOrLoadedIdeo);
				this.customOrLoadedIdeo = null;
			}
		}

		// Token: 0x0600BB8A RID: 48010 RVA: 0x00428EE0 File Offset: 0x004270E0
		private void CheckRemoveNewIdeoAndMakePrimary(Ideo primaryIdeo)
		{
			this.MakeIdeoPrimary(primaryIdeo);
			this.RemoveNewIdeoIfCreated();
			SoundDefOf.Click.PlayOneShotOnCamera(null);
		}

		// Token: 0x0600BB8B RID: 48011 RVA: 0x00428EFC File Offset: 0x004270FC
		private void MakeIdeoPrimary(Ideo primaryIdeo)
		{
			if (!Find.IdeoManager.IdeosListForReading.Contains(primaryIdeo))
			{
				Find.IdeoManager.Add(primaryIdeo);
			}
			foreach (Pawn pawn in this.pawns)
			{
				if (this.pawnConvertToIdeo[pawn] == this.CurrentPrimaryIdeo)
				{
					this.pawnConvertToIdeo[pawn] = primaryIdeo;
				}
				else if (this.pawnConvertToIdeo[pawn] == null && pawn.Ideo == this.initialPrimaryIdeo)
				{
					this.pawnConvertToIdeo[pawn] = primaryIdeo;
				}
			}
			this.newPrimaryIdeo = primaryIdeo;
			IdeoUIUtility.SetSelected(primaryIdeo);
		}

		// Token: 0x04006AD9 RID: 27353
		public const float TitleAreaHeight = 45f;

		// Token: 0x04006ADA RID: 27354
		private static readonly Vector2 ChangeOrDeleteIdeoBtnSize = new Vector2(250f, 40f);

		// Token: 0x04006ADB RID: 27355
		private static readonly Vector2 AssignColonistsBtnSize = new Vector2(250f, 40f);

		// Token: 0x04006ADC RID: 27356
		private Action nextAction;

		// Token: 0x04006ADD RID: 27357
		private bool forArchonexusRestart;

		// Token: 0x04006ADE RID: 27358
		private Vector2 scrollPosition_ideoList;

		// Token: 0x04006ADF RID: 27359
		private float scrollViewHeight_ideoList;

		// Token: 0x04006AE0 RID: 27360
		private Vector2 scrollPosition_ideoDetails;

		// Token: 0x04006AE1 RID: 27361
		private float scrollViewHeight_ideoDetails;

		// Token: 0x04006AE2 RID: 27362
		private Ideo initialPrimaryIdeo;

		// Token: 0x04006AE3 RID: 27363
		private Ideo newPrimaryIdeo;

		// Token: 0x04006AE4 RID: 27364
		private Ideo customOrLoadedIdeo;

		// Token: 0x04006AE5 RID: 27365
		private Dictionary<Pawn, Ideo> pawnConvertToIdeo = new Dictionary<Pawn, Ideo>();

		// Token: 0x04006AE6 RID: 27366
		private List<Pawn> pawns = new List<Pawn>();
	}
}
