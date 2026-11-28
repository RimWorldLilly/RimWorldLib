using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001D04 RID: 7428
	public class Dialog_ConfirmModUpload : Dialog_MessageBox
	{
		// Token: 0x0600BFF7 RID: 49143 RVA: 0x0045ECD4 File Offset: 0x0045CED4
		public Dialog_ConfirmModUpload(ModMetaData mod, Action acceptAction) : base("ConfirmSteamWorkshopUpload".Translate(), "Confirm".Translate(), acceptAction, "GoBack".Translate(), null, null, true, acceptAction, null, WindowLayer.Dialog)
		{
			this.mod = mod;
		}

		// Token: 0x0600BFF8 RID: 49144 RVA: 0x0045ED20 File Offset: 0x0045CF20
		public override void DoWindowContents(Rect inRect)
		{
			base.DoWindowContents(inRect);
			Vector2 vector = new Vector2(inRect.x + 10f, inRect.height - 35f - 24f - 10f);
			Widgets.Checkbox(vector, ref this.mod.translationMod, 24f, false, false, null, null);
			Widgets.Label(new Rect(vector.x + 24f + 10f, vector.y + (24f - Text.LineHeight) / 2f, inRect.width / 2f, 24f), "TagAsTranslation".Translate());
			vector.y -= 34f;
			if (!this.mod.uploadBuildAssetPack)
			{
				this.mod.uploadSkipSourceTextures = false;
			}
			Dialog_ConfirmModUpload.AssetPackOption(vector, inRect, ref this.mod.uploadSkipSourceTextures, "UploadSkipSourceTextures", !this.mod.uploadBuildAssetPack);
			vector.y -= 34f;
			Dialog_ConfirmModUpload.AssetPackOption(vector, inRect, ref this.mod.uploadBuildAssetPack, "UploadBuildAssetPack", false);
		}

		// Token: 0x0600BFF9 RID: 49145 RVA: 0x0045EE44 File Offset: 0x0045D044
		private static void AssetPackOption(Vector2 checkBoxPos, Rect inRect, ref bool value, string key, bool disabled)
		{
			Widgets.Checkbox(checkBoxPos, ref value, 24f, disabled, false, null, null);
			Rect rect = new Rect(checkBoxPos.x + 24f + 10f, checkBoxPos.y + (24f - Text.LineHeight) / 2f, inRect.width / 2f, 24f);
			if (disabled)
			{
				GUI.color = Color.gray;
			}
			Widgets.Label(rect, key.Translate());
			GUI.color = Color.white;
			TooltipHandler.TipRegion(new Rect(checkBoxPos.x, checkBoxPos.y, 34f + rect.width, 24f), (key + "Tip").Translate());
		}

		// Token: 0x04006E88 RID: 28296
		private ModMetaData mod;
	}
}
