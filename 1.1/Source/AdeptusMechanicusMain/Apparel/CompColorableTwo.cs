﻿using RimWorld;
using System;
using UnityEngine;
using Verse;

namespace AdeptusMechanicus
{
	// Token: 0x0200031C RID: 796
	public class CompColorableTwo : CompColorable
	{
		public new virtual Color Color
		{
			get
			{
			//	Log.Message(this.parent.LabelCap + " CompColorableTwo color active: " + active);
				if (!this.active)
				{
					return this.parent.def.graphicData.color;
				}
				return this.color;
			}
			set
			{
				if (value == this.color)
				{
					return;
				}
				this.active = true;
				this.color = value;
			//	Log.Message(this.parent.LabelCap + " CompColorableTwo color set: " + value);
				this.parent.Notify_ColorChanged();
			}
		}

		public virtual Color ColorTwo
		{
			get
			{
			//	Log.Message(this.parent.LabelCap + " CompColorableTwo colorTwo active: " + ActiveTwo);
				if (!this.activeTwo)
				{
					return this.parent.def.graphicData.colorTwo;
				}
				return this.colorTwo;
			}
			set
			{
				if (value == this.colorTwo)
				{
					return;
				}
				this.ActiveTwo = true;
				this.colorTwo = value;
			//	Log.Message(this.parent.LabelCap + " CompColorableTwo colorTwo set: " + value);
				this.parent.Notify_ColorChanged();
			}
		}

		// Token: 0x170004CD RID: 1229
		// (get) Token: 0x06001743 RID: 5955 RVA: 0x00085558 File Offset: 0x00083758
		public new virtual bool Active
		{
			get
			{
				return this.active;
			}
			set
			{
				this.active = value;
			}
		}
		
		public virtual bool ActiveTwo
		{
			get
			{
				return this.activeTwo;
			}
			set
			{

				this.activeTwo = value;
			}
		}

		// Token: 0x06001744 RID: 5956 RVA: 0x00085560 File Offset: 0x00083760
		public override void Initialize(CompProperties props)
		{
			this.props = props;
			if (this.parent.def.colorGenerator != null && (this.parent.Stuff == null || this.parent.Stuff.stuffProps.allowColorGenerators) && !this.active)
			{
				this.Color = this.parent.def.colorGenerator.NewRandomizedColor();
				this.active = true;
				//	Log.Message("getting new random colourtwo "+ this.Color);
			}
			if (this.parent.def.colorGenerator != null && (this.parent.Stuff == null || this.parent.Stuff.stuffProps.allowColorGenerators) && !this.activeTwo)
			{
				this.ColorTwo = this.parent.def.colorGenerator.NewRandomizedColor();
				this.activeTwo = true;
				//	Log.Message("getting new random colourtwo "+ this.Color);
			}
		}

		public override void PostPostMake()
		{
			if (this.parent.def.colorGenerator != null && (this.parent.Stuff == null || this.parent.Stuff.stuffProps.allowColorGenerators) && !this.active)
			{
				this.Color = this.parent.def.colorGenerator.NewRandomizedColor();
				this.active = true;
				//	Log.Message("getting new random colourtwo "+ this.Color);
			}
			if (this.parent.def.colorGenerator != null && (this.parent.Stuff == null || this.parent.Stuff.stuffProps.allowColorGenerators) && !this.activeTwo)
			{
				this.ColorTwo = this.parent.def.colorGenerator.NewRandomizedColor();
				this.activeTwo = true;
				//	Log.Message("getting new random colourtwo "+ this.Color);
			}
		}

		// Token: 0x06001745 RID: 5957 RVA: 0x000855C8 File Offset: 0x000837C8
		public override void PostExposeData()
		{
			Scribe_Values.Look<Color>(ref this.color, "color", default(Color), false);
			Scribe_Values.Look<Color>(ref this.colorTwo, "colorTwo", default(Color), false);
			Scribe_Values.Look<bool>(ref this.active, "Active", false, false);
			Scribe_Values.Look<bool>(ref this.activeTwo, "ActiveTwo", false, false);
		}

		// Token: 0x06001746 RID: 5958 RVA: 0x00085618 File Offset: 0x00083818
		public override void PostSplitOff(Thing piece)
		{
			base.PostSplitOff(piece);
			if (this.active)
			{
				if (this.color != null && this.color != Color.white)
				{
					piece.SetColor(this.color, true);
				}
				if (this.colorTwo != null && this.colorTwo != Color.white)
				{
					piece.SetColorTwo(this.colorTwo, true);
				}
			}
		}

		// Token: 0x04000EA1 RID: 3745
		protected Color color = Color.white;
		protected Color colorTwo = Color.white;

		// Token: 0x04000EA2 RID: 3746
		protected bool active;
		protected bool activeTwo;
	}
}
