using System;
using Steamworks;
using UnityEngine;

namespace Verse.Steam
{
	// Token: 0x02000744 RID: 1860
	public class Dialog_WorkshopOperationInProgress : Window
	{
		// Token: 0x17000B7F RID: 2943
		// (get) Token: 0x06003AF3 RID: 15091 RVA: 0x001782ED File Offset: 0x001764ED
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(600f, 400f);
			}
		}

		// Token: 0x06003AF4 RID: 15092 RVA: 0x001782FE File Offset: 0x001764FE
		public Dialog_WorkshopOperationInProgress() : base(null)
		{
			this.forcePause = true;
			this.closeOnAccept = false;
			this.closeOnCancel = false;
			this.absorbInputAroundWindow = true;
			this.preventDrawTutor = true;
		}

		// Token: 0x06003AF5 RID: 15093 RVA: 0x0017832C File Offset: 0x0017652C
		public override void DoWindowContents(Rect inRect)
		{
			EItemUpdateStatus eitemUpdateStatus;
			float num;
			Workshop.GetUpdateStatus(out eitemUpdateStatus, out num);
			WorkshopInteractStage curStage = Workshop.CurStage;
			if (curStage == WorkshopInteractStage.None && eitemUpdateStatus == EItemUpdateStatus.k_EItemUpdateStatusInvalid)
			{
				this.Close(true);
				return;
			}
			string text = "";
			if (curStage != WorkshopInteractStage.None)
			{
				text += curStage.GetLabel();
				text += "\n\n";
			}
			if (eitemUpdateStatus != EItemUpdateStatus.k_EItemUpdateStatusInvalid)
			{
				text += eitemUpdateStatus.GetLabel();
				if (num > 0f)
				{
					text = text + " (" + num.ToStringPercent() + ")";
				}
				text += GenText.MarchingEllipsis(0f);
			}
			Widgets.Label(inRect, text);
		}

		// Token: 0x06003AF6 RID: 15094 RVA: 0x001783C0 File Offset: 0x001765C0
		public static void CloseAll()
		{
			Dialog_WorkshopOperationInProgress dialog_WorkshopOperationInProgress = Find.WindowStack.WindowOfType<Dialog_WorkshopOperationInProgress>();
			if (dialog_WorkshopOperationInProgress != null)
			{
				dialog_WorkshopOperationInProgress.Close(true);
			}
		}
	}
}
