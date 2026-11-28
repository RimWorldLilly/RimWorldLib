using System;
using System.Collections.Generic;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001CCC RID: 7372
	public class Dialog_WorldSearch : Dialog_Search<WorldSearchElement>
	{
		// Token: 0x17002283 RID: 8835
		// (get) Token: 0x0600BD97 RID: 48535 RVA: 0x00441B15 File Offset: 0x0043FD15
		protected override bool ShouldClose
		{
			get
			{
				return WorldRendererUtility.DrawingMap;
			}
		}

		// Token: 0x17002284 RID: 8836
		// (get) Token: 0x0600BD98 RID: 48536 RVA: 0x00441B1C File Offset: 0x0043FD1C
		protected override List<WorldSearchElement> SearchSet
		{
			get
			{
				return this.searchSet;
			}
		}

		// Token: 0x17002285 RID: 8837
		// (get) Token: 0x0600BD99 RID: 48537 RVA: 0x00441B24 File Offset: 0x0043FD24
		protected override TaggedString SearchLabel
		{
			get
			{
				return "SearchTheWorld".Translate();
			}
		}

		// Token: 0x0600BD9A RID: 48538 RVA: 0x00441B30 File Offset: 0x0043FD30
		public bool IsListed(WorldObject wo)
		{
			return this.listedWorldObjects.Contains(wo);
		}

		// Token: 0x0600BD9B RID: 48539 RVA: 0x00441B3E File Offset: 0x0043FD3E
		public bool IsListed(PlanetTile tile)
		{
			return this.listedTiles.Contains(tile);
		}

		// Token: 0x0600BD9C RID: 48540 RVA: 0x00441B4C File Offset: 0x0043FD4C
		public Dialog_WorldSearch()
		{
			this.searchSet = new List<WorldSearchElement>();
			this.InitializeSearchSet();
		}

		// Token: 0x0600BD9D RID: 48541 RVA: 0x00441B7C File Offset: 0x0043FD7C
		private void InitializeSearchSet()
		{
			this.searchSet.Clear();
			foreach (WorldObject worldObject in Find.World.worldObjects.AllWorldObjects)
			{
				WorldSearchElement worldSearchElement = new WorldSearchElement
				{
					tile = worldObject.Tile,
					worldObject = worldObject
				};
				if (ModsConfig.OdysseyActive)
				{
					worldSearchElement.landmark = Find.World.landmarks.landmarks.TryGetValue(worldObject.Tile, null);
				}
				worldSearchElement.mutators = Find.World.grid[worldObject.Tile].mutatorsNullable;
				this.searchSet.Add(worldSearchElement);
			}
			if (ModsConfig.OdysseyActive)
			{
				using (Dictionary<PlanetTile, Landmark>.Enumerator enumerator2 = Find.World.landmarks.landmarks.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						KeyValuePair<PlanetTile, Landmark> landmark = enumerator2.Current;
						if (!this.searchSet.Any((WorldSearchElement o) => o.tile == landmark.Key))
						{
							WorldSearchElement item = new WorldSearchElement
							{
								tile = landmark.Key,
								landmark = landmark.Value,
								mutators = Find.World.grid[landmark.Key].mutatorsNullable
							};
							this.searchSet.Add(item);
						}
					}
				}
			}
		}

		// Token: 0x0600BD9E RID: 48542 RVA: 0x00441D24 File Offset: 0x0043FF24
		protected override void TryAddElement(WorldSearchElement element)
		{
			if (!this.searchResultsSet.Add(element))
			{
				return;
			}
			if (this.ElementMatch(element))
			{
				this.searchResults.Add(element.DisplayLabel.ToLower(), element);
				if (element.worldObject != null)
				{
					this.listedWorldObjects.Add(element.worldObject);
				}
				if (element.tile.Valid)
				{
					this.listedTiles.Add(element.tile);
				}
				this.SetInitialSizeAndPosition();
			}
		}

		// Token: 0x0600BD9F RID: 48543 RVA: 0x00441D9F File Offset: 0x0043FF9F
		public override void Notify_CommonSearchChanged()
		{
			base.Notify_CommonSearchChanged();
			this.listedWorldObjects.Clear();
			this.listedTiles.Clear();
		}

		// Token: 0x0600BDA0 RID: 48544 RVA: 0x00441DC0 File Offset: 0x0043FFC0
		private bool ElementMatch(WorldSearchElement element)
		{
			if (element.worldObject != null && base.TextMatch(element.worldObject.Label))
			{
				return true;
			}
			if (element.landmark != null)
			{
				if (base.TextMatch(element.landmark.name))
				{
					return true;
				}
				if (base.TextMatch(element.landmark.def.label))
				{
					return true;
				}
			}
			if (!element.mutators.NullOrEmpty<TileMutatorDef>())
			{
				foreach (TileMutatorDef tileMutatorDef in element.mutators)
				{
					if (base.TextMatch(tileMutatorDef.label))
					{
						return true;
					}
					if (base.TextMatch(tileMutatorDef.Label(element.tile)))
					{
						return true;
					}
				}
				return false;
			}
			return false;
		}

		// Token: 0x0600BDA1 RID: 48545 RVA: 0x00441E9C File Offset: 0x0044009C
		protected override void TryRemoveElement(WorldSearchElement element)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600BDA2 RID: 48546 RVA: 0x00441EA3 File Offset: 0x004400A3
		protected override void DoIcon(WorldSearchElement element, Rect iconRect)
		{
			if (element.worldObject != null)
			{
				GUI.color = element.worldObject.ExpandingIconColor;
				GUI.DrawTexture(iconRect, element.worldObject.ExpandingIcon);
				GUI.color = Color.white;
			}
		}

		// Token: 0x0600BDA3 RID: 48547 RVA: 0x00441ED8 File Offset: 0x004400D8
		protected override void DoLabel(WorldSearchElement element, Rect labelRect)
		{
			Widgets.Label(labelRect, element.DisplayLabel);
		}

		// Token: 0x0600BDA4 RID: 48548 RVA: 0x00441EE8 File Offset: 0x004400E8
		protected override void ClikedOnElement(WorldSearchElement element)
		{
			Find.WorldSelector.ClearSelection();
			if (element.worldObject != null)
			{
				Find.WorldSelector.Select(element.worldObject, true);
			}
			else
			{
				Find.WorldSelector.SelectedTile = element.tile;
			}
			Find.WorldCameraDriver.JumpTo(element.tile);
		}

		// Token: 0x0600BDA5 RID: 48549 RVA: 0x00441F3A File Offset: 0x0044013A
		protected override bool ShouldSkipElement(WorldSearchElement element)
		{
			return element.DisplayLabel.NullOrEmpty();
		}

		// Token: 0x0600BDA6 RID: 48550 RVA: 0x00441F47 File Offset: 0x00440147
		protected override void OnHighlightUpdate(WorldSearchElement element)
		{
		}

		// Token: 0x04006C88 RID: 27784
		private readonly List<WorldSearchElement> searchSet;

		// Token: 0x04006C89 RID: 27785
		private readonly HashSet<WorldObject> listedWorldObjects = new HashSet<WorldObject>();

		// Token: 0x04006C8A RID: 27786
		private readonly HashSet<PlanetTile> listedTiles = new HashSet<PlanetTile>();
	}
}
