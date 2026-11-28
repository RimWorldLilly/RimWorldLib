using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001D0F RID: 7439
	[StaticConstructorOnStartup]
	public class UI_BackgroundMain : UIMenuBackground
	{
		// Token: 0x170022F3 RID: 8947
		// (get) Token: 0x0600C05D RID: 49245 RVA: 0x00462B58 File Offset: 0x00460D58
		private float DeltaAlpha
		{
			get
			{
				return Time.deltaTime * 2f;
			}
		}

		// Token: 0x0600C05E RID: 49246 RVA: 0x00462B68 File Offset: 0x00460D68
		public void SetupExpansionFadeData()
		{
			this.expansionImageFades = new Dictionary<ExpansionDef, float>();
			foreach (ExpansionDef key in ModLister.AllExpansions)
			{
				this.expansionImageFades.Add(key, 0f);
			}
		}

		// Token: 0x0600C05F RID: 49247 RVA: 0x00462BD0 File Offset: 0x00460DD0
		public override void BackgroundOnGUI()
		{
			Vector2 vector = (this.overrideBGImage != null) ? new Vector2((float)this.overrideBGImage.width, (float)this.overrideBGImage.height) : UI_BackgroundMain.BGPlanetSize;
			bool flag = true;
			if ((float)UI.screenWidth > (float)UI.screenHeight * (vector.x / vector.y))
			{
				flag = false;
			}
			Rect rect;
			if (flag)
			{
				float height = (float)UI.screenHeight;
				float num = (float)UI.screenHeight * (vector.x / vector.y);
				rect = new Rect((float)(UI.screenWidth / 2) - num / 2f, 0f, num, height);
			}
			else
			{
				float width = (float)UI.screenWidth;
				float num2 = (float)UI.screenWidth * (vector.y / vector.x);
				rect = new Rect(0f, (float)(UI.screenHeight / 2) - num2 / 2f, width, num2);
			}
			GUI.DrawTexture(rect, this.overrideBGImage ?? UI_BackgroundMain.BGPlanet, ScaleMode.ScaleToFit);
			if (Event.current.type == EventType.repaint)
			{
				this.DoOverlay(rect);
			}
		}

		// Token: 0x0600C060 RID: 49248 RVA: 0x00462CDC File Offset: 0x00460EDC
		private void DoOverlay(Rect bgRect)
		{
			if (this.expansionImageFades == null)
			{
				return;
			}
			foreach (ExpansionDef expansionDef in ModLister.AllExpansions)
			{
				if (!expansionDef.isCore && !expansionDef.BackgroundImage.NullOrBad() && this.expansionImageFades[expansionDef] > 0f)
				{
					if (expansionDef.BackgroundImage != this.overrideBGImage)
					{
						GUI.color = new Color(1f, 1f, 1f, this.expansionImageFades[expansionDef]);
						GUI.DrawTexture(bgRect, expansionDef.BackgroundImage, ScaleMode.ScaleAndCrop);
						GUI.color = Color.white;
					}
					this.expansionImageFades[expansionDef] = Mathf.Clamp01(this.expansionImageFades[expansionDef] - this.DeltaAlpha / 2f);
				}
			}
		}

		// Token: 0x0600C061 RID: 49249 RVA: 0x00462DE0 File Offset: 0x00460FE0
		public void Notify_Hovered(ExpansionDef expansionDef)
		{
			if (Event.current.type == EventType.repaint)
			{
				this.expansionImageFades[expansionDef] = Mathf.Clamp01(this.expansionImageFades[expansionDef] + this.DeltaAlpha);
			}
		}

		// Token: 0x04006EC7 RID: 28359
		public Texture2D overrideBGImage;

		// Token: 0x04006EC8 RID: 28360
		private Dictionary<ExpansionDef, float> expansionImageFades;

		// Token: 0x04006EC9 RID: 28361
		private static readonly Vector2 BGPlanetSize = new Vector2(2048f, 1280f);

		// Token: 0x04006ECA RID: 28362
		private static readonly Texture2D BGPlanet = ContentFinder<Texture2D>.Get("UI/HeroArt/BGPlanet", true);
	}
}
