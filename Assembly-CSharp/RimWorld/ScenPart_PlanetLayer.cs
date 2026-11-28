using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020017B2 RID: 6066
	public class ScenPart_PlanetLayer : ScenPart
	{
		// Token: 0x170019B3 RID: 6579
		// (get) Token: 0x06009723 RID: 38691 RVA: 0x0036CFF0 File Offset: 0x0036B1F0
		public PlanetLayerSettings Settings
		{
			get
			{
				return this.settingsDef.settings;
			}
		}

		// Token: 0x06009724 RID: 38692 RVA: 0x0036CFFD File Offset: 0x0036B1FD
		public override bool Valid()
		{
			return base.Valid() && this.settingsDef != null;
		}

		// Token: 0x170019B4 RID: 6580
		// (get) Token: 0x06009725 RID: 38693 RVA: 0x0036D012 File Offset: 0x0036B212
		protected virtual bool CanEdit
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06009726 RID: 38694 RVA: 0x0036D018 File Offset: 0x0036B218
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<PlanetLayerDef>(ref this.layer, "layer");
			Scribe_Defs.Look<PlanetLayerSettingsDef>(ref this.settingsDef, "settingsDef");
			Scribe_Values.Look<bool>(ref this.hide, "hide", false, false);
			Scribe_Values.Look<string>(ref this.tag, "tag", null, false);
			Scribe_Collections.Look<LayerConnection>(ref this.connections, "connections", LookMode.Deep, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				if (this.layer == null)
				{
					Log.Error("ScenPart has null layer after loading.");
				}
				if (this.settingsDef == null)
				{
					Log.Error("ScenPart has null playet layer settings def.");
				}
			}
		}

		// Token: 0x06009727 RID: 38695 RVA: 0x0036D0B4 File Offset: 0x0036B2B4
		public override int GetHashCode()
		{
			HashCode hashCode = default(HashCode);
			hashCode.Add<int>(base.GetHashCode());
			hashCode.Add<int>((this.layer != null) ? this.layer.GetHashCode() : 0);
			hashCode.Add<int>((this.settingsDef != null) ? this.settingsDef.GetHashCode() : 0);
			hashCode.Add<string>(this.tag);
			foreach (LayerConnection layerConnection in this.connections)
			{
				hashCode.Add<int>(layerConnection.GetHashCode());
			}
			return hashCode.ToHashCode();
		}

		// Token: 0x06009728 RID: 38696 RVA: 0x0036D170 File Offset: 0x0036B370
		public override IEnumerable<string> ConfigErrors()
		{
			ScenPart_PlanetLayer.<ConfigErrors>d__12 <ConfigErrors>d__ = new ScenPart_PlanetLayer.<ConfigErrors>d__12(-2);
			<ConfigErrors>d__.<>4__this = this;
			return <ConfigErrors>d__;
		}

		// Token: 0x06009729 RID: 38697 RVA: 0x0036D180 File Offset: 0x0036B380
		public override void DoEditInterface(Listing_ScenEdit listing)
		{
			if (this.hide)
			{
				return;
			}
			this.ClearInvalidConnections();
			Rect rect = listing.GetScenPartRect(this, this.lastHeight).TopPartPixels(ScenPart.RowHeight);
			float yMin = rect.yMin;
			this.DoTypeRow(ref rect);
			this.DoTagRow(ref rect);
			this.DoSettings(ref rect);
			this.DoConnections(ref rect);
			this.lastHeight = rect.yMax - yMin + 4f;
		}

		// Token: 0x0600972A RID: 38698 RVA: 0x0036D1F4 File Offset: 0x0036B3F4
		private bool IsTagValid()
		{
			if (string.IsNullOrEmpty(this.tag))
			{
				return false;
			}
			using (List<string>.Enumerator enumerator = this.GetAllTags().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current == this.tag)
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x0600972B RID: 38699 RVA: 0x0036D264 File Offset: 0x0036B464
		public override bool TryMerge(ScenPart other)
		{
			return false;
		}

		// Token: 0x0600972C RID: 38700 RVA: 0x0036D268 File Offset: 0x0036B468
		public override bool CanCoexistWith(ScenPart other)
		{
			ScenPart_PlanetLayer scenPart_PlanetLayer = other as ScenPart_PlanetLayer;
			return scenPart_PlanetLayer == null || !(scenPart_PlanetLayer.tag == this.tag);
		}

		// Token: 0x0600972D RID: 38701 RVA: 0x0036D298 File Offset: 0x0036B498
		private List<string> GetAllTags()
		{
			ScenPart_PlanetLayer.tmpTags.Clear();
			foreach (ScenPart scenPart in Find.Scenario.AllParts)
			{
				if (scenPart != this)
				{
					ScenPart_PlanetLayer scenPart_PlanetLayer = scenPart as ScenPart_PlanetLayer;
					if (scenPart_PlanetLayer != null && !string.IsNullOrEmpty(scenPart_PlanetLayer.tag))
					{
						ScenPart_PlanetLayer.tmpTags.Add(scenPart_PlanetLayer.tag);
					}
				}
			}
			return ScenPart_PlanetLayer.tmpTags;
		}

		// Token: 0x0600972E RID: 38702 RVA: 0x0036D31C File Offset: 0x0036B51C
		private void DoLabel(Rect row, string label)
		{
			Rect rect = row;
			rect.x -= rect.width + 5f;
			using (new TextBlock(TextAnchor.MiddleRight))
			{
				Widgets.Label(rect, label);
			}
		}

		// Token: 0x0600972F RID: 38703 RVA: 0x0036D378 File Offset: 0x0036B578
		private void DoTypeRow(ref Rect row)
		{
			using (new TextBlock(this.CanEdit ? Color.white : Color.gray))
			{
				Rect rect = row;
				PlanetLayerDef planetLayerDef = this.layer;
				if (Widgets.ButtonText(rect, (planetLayerDef != null) ? planetLayerDef.LabelCap : "ScenPart_SelectQuestDef".Translate(), true, true, this.CanEdit, null))
				{
					FloatMenuUtility.MakeMenu<PlanetLayerDef>(DefDatabase<PlanetLayerDef>.AllDefs, (PlanetLayerDef id) => id.LabelCap, (PlanetLayerDef selected) => delegate()
					{
						this.layer = selected;
					});
				}
				row.y += ScenPart.RowHeight + 4f;
			}
		}

		// Token: 0x06009730 RID: 38704 RVA: 0x0036D44C File Offset: 0x0036B64C
		private void DoTagRow(ref Rect row)
		{
			string text = this.tag;
			this.tag = Widgets.TextArea(row, this.tag, false);
			if (!this.IsTagValid())
			{
				using (new TextBlock(ColorLibrary.RedReadable))
				{
					this.DoLabel(row, "ScenPart_PlanetLayerTag_Error".Translate());
					goto IL_79;
				}
			}
			this.DoLabel(row, "ScenPart_PlanetLayerTag".Translate());
			IL_79:
			if (text != this.tag && !string.IsNullOrEmpty(this.tag))
			{
				foreach (ScenPart scenPart in Find.Scenario.AllParts)
				{
					if (scenPart != this)
					{
						ScenPart_PlanetLayer scenPart_PlanetLayer = scenPart as ScenPart_PlanetLayer;
						if (scenPart_PlanetLayer != null)
						{
							foreach (LayerConnection layerConnection in scenPart_PlanetLayer.connections)
							{
								if (layerConnection.tag == text)
								{
									layerConnection.tag = this.tag;
								}
							}
						}
					}
				}
			}
			row.y += ScenPart.RowHeight + 4f;
		}

		// Token: 0x06009731 RID: 38705 RVA: 0x0036D5C0 File Offset: 0x0036B7C0
		private void DoSettings(ref Rect row)
		{
			this.Seperator(ref row);
			this.DoLabel(row, "ScenPart_PlanetLayerSettings".Translate());
			PlanetLayerSettingsDef planetLayerSettingsDef = this.settingsDef;
			TaggedString taggedString = (planetLayerSettingsDef != null) ? planetLayerSettingsDef.LabelCap : "ScenPart_PlanetLayerSettings_SelectDef".Translate();
			if (Widgets.ButtonText(row, taggedString, true, true, true, null))
			{
				FloatMenuUtility.MakeMenu<PlanetLayerSettingsDef>(DefDatabase<PlanetLayerSettingsDef>.AllDefs, (PlanetLayerSettingsDef x) => x.LabelCap, (PlanetLayerSettingsDef x) => delegate()
				{
					this.settingsDef = x;
				});
			}
			row.y += ScenPart.RowHeight + 4f;
		}

		// Token: 0x06009732 RID: 38706 RVA: 0x0036D678 File Offset: 0x0036B878
		private void DoConnections(ref Rect row)
		{
			this.Seperator(ref row);
			this.DoLabel(row, "ScenPart_PlanetLayerConnections".Translate());
			for (int i = this.connections.Count - 1; i >= 0; i--)
			{
				if (i % 2 == 1)
				{
					Widgets.DrawLightHighlight(row);
				}
				LayerConnection other = this.connections[i];
				Rect rect = row.LeftPart(0.25f);
				Rect rect2 = row.LeftPart(0.25f);
				Rect rect3 = row.RightPart(0.5f);
				rect2.x = rect.xMax;
				rect.xMax -= 4f;
				rect2.xMax += 4f;
				rect2.xMax -= 4f;
				rect3.xMin += 4f;
				if (Widgets.ButtonText(rect, other.tag, true, true, true, null))
				{
					List<FloatMenuOption> list = new List<FloatMenuOption>();
					int captured = i;
					list.Add(new FloatMenuOption("Remove", delegate()
					{
						this.connections.RemoveAt(captured);
					}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
					using (List<string>.Enumerator enumerator = this.GetAllTags().GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							string value = enumerator.Current;
							if (!this.connections.Any((LayerConnection x) => x.tag == value))
							{
								list.Add(new FloatMenuOption(value, delegate()
								{
									other.tag = value;
								}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
							}
						}
					}
					Find.WindowStack.Add(new FloatMenu(list));
				}
				string label;
				if (other.zoomMode == LayerConnection.ZoomMode.None)
				{
					label = "ScenPart_PlanetLayerConnections_Zoom".Translate();
				}
				else if (other.zoomMode == LayerConnection.ZoomMode.ZoomIn)
				{
					label = "ScenPart_PlanetLayerConnections_ZoomIn".Translate();
				}
				else
				{
					label = "ScenPart_PlanetLayerConnections_ZoomOut".Translate();
				}
				if (Widgets.ButtonText(rect2, label, true, true, true, null))
				{
					List<FloatMenuOption> list2 = new List<FloatMenuOption>();
					int localIndex = i;
					if (other.zoomMode != LayerConnection.ZoomMode.None)
					{
						list2.Add(new FloatMenuOption("ScenPart_PlanetLayerConnections_Zoom".Translate(), delegate()
						{
							other.zoomMode = LayerConnection.ZoomMode.None;
						}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
					}
					if (other.zoomMode != LayerConnection.ZoomMode.ZoomIn)
					{
						list2.Add(new FloatMenuOption("ScenPart_PlanetLayerConnections_ZoomIn".Translate(), delegate()
						{
							other.zoomMode = LayerConnection.ZoomMode.ZoomIn;
							for (int j = 0; j < this.connections.Count; j++)
							{
								if (localIndex != j && this.connections[j].zoomMode == LayerConnection.ZoomMode.ZoomIn)
								{
									this.connections[j].zoomMode = LayerConnection.ZoomMode.None;
								}
							}
						}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
					}
					if (other.zoomMode != LayerConnection.ZoomMode.ZoomOut)
					{
						list2.Add(new FloatMenuOption("ScenPart_PlanetLayerConnections_ZoomOut".Translate(), delegate()
						{
							other.zoomMode = LayerConnection.ZoomMode.ZoomOut;
							for (int j = 0; j < this.connections.Count; j++)
							{
								if (localIndex != j && this.connections[j].zoomMode == LayerConnection.ZoomMode.ZoomOut)
								{
									this.connections[j].zoomMode = LayerConnection.ZoomMode.None;
								}
							}
						}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
					}
					Find.WindowStack.Add(new FloatMenu(list2));
				}
				Rect rect4 = rect3.LeftHalf();
				rect4.xMax -= 4f;
				using (new TextBlock(TextAnchor.MiddleRight))
				{
					Widgets.Label(rect4, "ScenPart_PlanetLayerConnections_Fuel".Translate());
				}
				Widgets.TextFieldNumeric<float>(rect3.RightHalf(), ref other.fuelCost, ref this.numericBuffer, 0f, 1E+09f);
				row.y += ScenPart.RowHeight + 4f;
			}
			List<string> unusedTags = this.GetUnusedTags();
			using (new TextBlock(unusedTags.Any<string>() ? Color.white : Color.gray))
			{
				if (Widgets.ButtonText(row, "ScenPart_PlanetLayerConnections_Add".Translate(), true, true, unusedTags.Any<string>(), null))
				{
					FloatMenuUtility.MakeMenu<string>(unusedTags, (string id) => id, (string nTag) => delegate()
					{
						this.connections.Add(new LayerConnection
						{
							tag = nTag
						});
					});
				}
			}
		}

		// Token: 0x06009733 RID: 38707 RVA: 0x0036DB44 File Offset: 0x0036BD44
		private void Seperator(ref Rect row)
		{
			Widgets.DrawLineHorizontal(row.x + 4f, row.y + row.height / 4f, row.width - 8f, Color.gray);
			row.y += 17f;
		}

		// Token: 0x06009734 RID: 38708 RVA: 0x0036DB98 File Offset: 0x0036BD98
		private List<string> GetUnusedTags()
		{
			ScenPart_PlanetLayer.tmpTags.Clear();
			foreach (ScenPart scenPart in Find.Scenario.AllParts)
			{
				if (scenPart != this)
				{
					ScenPart_PlanetLayer other = scenPart as ScenPart_PlanetLayer;
					if (other != null && !string.IsNullOrEmpty(other.tag) && !this.connections.Any((LayerConnection t) => t.tag == other.tag))
					{
						ScenPart_PlanetLayer.tmpTags.Add(other.tag);
					}
				}
			}
			return ScenPart_PlanetLayer.tmpTags;
		}

		// Token: 0x06009735 RID: 38709 RVA: 0x0036DC50 File Offset: 0x0036BE50
		private void ClearInvalidConnections()
		{
			for (int i = this.connections.Count - 1; i >= 0; i--)
			{
				LayerConnection layerConnection = this.connections[i];
				bool flag = false;
				foreach (ScenPart scenPart in Find.Scenario.AllParts)
				{
					if (this != scenPart)
					{
						ScenPart_PlanetLayer scenPart_PlanetLayer = scenPart as ScenPart_PlanetLayer;
						if (scenPart_PlanetLayer != null && scenPart_PlanetLayer.tag == layerConnection.tag)
						{
							flag = true;
							break;
						}
					}
				}
				if (!flag)
				{
					this.connections.RemoveAt(i);
				}
			}
		}

		// Token: 0x04005A8E RID: 23182
		public PlanetLayerDef layer;

		// Token: 0x04005A8F RID: 23183
		public PlanetLayerSettingsDef settingsDef;

		// Token: 0x04005A90 RID: 23184
		public bool hide;

		// Token: 0x04005A91 RID: 23185
		public string tag;

		// Token: 0x04005A92 RID: 23186
		public List<LayerConnection> connections = new List<LayerConnection>();

		// Token: 0x04005A93 RID: 23187
		private float lastHeight;

		// Token: 0x04005A94 RID: 23188
		private static readonly List<string> tmpTags = new List<string>();

		// Token: 0x04005A95 RID: 23189
		private string[] vBuffer;

		// Token: 0x04005A96 RID: 23190
		private string radiusBuffer;

		// Token: 0x04005A97 RID: 23191
		private string extraCameraAltitudeBuffer;

		// Token: 0x04005A98 RID: 23192
		private string backgroundWorldCameraOffsetBuffer;

		// Token: 0x04005A99 RID: 23193
		private string backgroundWorldCameraParallaxBuffer;

		// Token: 0x04005A9A RID: 23194
		private string subdivisionsBuffer;

		// Token: 0x04005A9B RID: 23195
		private string viewAngleBuffer;

		// Token: 0x04005A9C RID: 23196
		private string numericBuffer;
	}
}
