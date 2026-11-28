using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x02000230 RID: 560
	public class AnimalPenEnclosureStateCalculator : AnimalPenEnclosureCalculator
	{
		// Token: 0x17000323 RID: 803
		// (get) Token: 0x06001043 RID: 4163 RVA: 0x0005D970 File Offset: 0x0005BB70
		public bool Enclosed
		{
			get
			{
				return this.enclosed;
			}
		}

		// Token: 0x17000324 RID: 804
		// (get) Token: 0x06001044 RID: 4164 RVA: 0x0005D978 File Offset: 0x0005BB78
		public bool IndirectlyConnected
		{
			get
			{
				return this.indirectlyConnected;
			}
		}

		// Token: 0x17000325 RID: 805
		// (get) Token: 0x06001045 RID: 4165 RVA: 0x0005D980 File Offset: 0x0005BB80
		public bool PassableDoors
		{
			get
			{
				return this.passableDoors.Any<Building_Door>();
			}
		}

		// Token: 0x17000326 RID: 806
		// (get) Token: 0x06001046 RID: 4166 RVA: 0x0005D98D File Offset: 0x0005BB8D
		public bool ImpassableDoors
		{
			get
			{
				return this.impassableDoors.Any<Building_Door>();
			}
		}

		// Token: 0x17000327 RID: 807
		// (get) Token: 0x06001047 RID: 4167 RVA: 0x0005D99A File Offset: 0x0005BB9A
		public List<Region> DirectlyConnectedRegions
		{
			get
			{
				return this.directlyConnectedRegions;
			}
		}

		// Token: 0x17000328 RID: 808
		// (get) Token: 0x06001048 RID: 4168 RVA: 0x0005D9A2 File Offset: 0x0005BBA2
		public HashSet<Region> ConnectedRegions
		{
			get
			{
				return this.connectedRegions;
			}
		}

		// Token: 0x06001049 RID: 4169 RVA: 0x0005D9AA File Offset: 0x0005BBAA
		public bool ContainsConnectedRegion(Region r)
		{
			return this.connectedRegions.Contains(r);
		}

		// Token: 0x0600104A RID: 4170 RVA: 0x0005D9B8 File Offset: 0x0005BBB8
		public bool NeedsRecalculation()
		{
			using (List<Building_Door>.Enumerator enumerator = this.passableDoors.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (!AnimalPenEnclosureCalculator.RoamerCanPass(enumerator.Current))
					{
						return true;
					}
				}
			}
			using (List<Building_Door>.Enumerator enumerator = this.impassableDoors.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (AnimalPenEnclosureCalculator.RoamerCanPass(enumerator.Current))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x0600104B RID: 4171 RVA: 0x0005DA58 File Offset: 0x0005BC58
		public void Recalulate(IntVec3 position, Map map)
		{
			this.indirectlyConnected = false;
			this.passableDoors.Clear();
			this.impassableDoors.Clear();
			this.connectedRegions.Clear();
			this.directlyConnectedRegions.Clear();
			this.enclosed = base.VisitPen(position, map);
		}

		// Token: 0x0600104C RID: 4172 RVA: 0x0005DAA6 File Offset: 0x0005BCA6
		protected override void VisitDirectlyConnectedRegion(Region r)
		{
			this.connectedRegions.Add(r);
			this.directlyConnectedRegions.Add(r);
		}

		// Token: 0x0600104D RID: 4173 RVA: 0x0005DAC1 File Offset: 0x0005BCC1
		protected override void VisitIndirectlyDirectlyConnectedRegion(Region r)
		{
			this.indirectlyConnected = true;
			this.connectedRegions.Add(r);
		}

		// Token: 0x0600104E RID: 4174 RVA: 0x0005DAD7 File Offset: 0x0005BCD7
		protected override void VisitPassableDoorway(Region r)
		{
			this.connectedRegions.Add(r);
			this.passableDoors.Add(r.door);
		}

		// Token: 0x0600104F RID: 4175 RVA: 0x0005DAF7 File Offset: 0x0005BCF7
		protected override void VisitImpassableDoorway(Region r)
		{
			this.impassableDoors.Add(r.door);
		}

		// Token: 0x04000F3B RID: 3899
		private bool enclosed;

		// Token: 0x04000F3C RID: 3900
		private bool indirectlyConnected;

		// Token: 0x04000F3D RID: 3901
		private List<Building_Door> passableDoors = new List<Building_Door>();

		// Token: 0x04000F3E RID: 3902
		private List<Building_Door> impassableDoors = new List<Building_Door>();

		// Token: 0x04000F3F RID: 3903
		private List<Region> directlyConnectedRegions = new List<Region>();

		// Token: 0x04000F40 RID: 3904
		private HashSet<Region> connectedRegions = new HashSet<Region>();
	}
}
