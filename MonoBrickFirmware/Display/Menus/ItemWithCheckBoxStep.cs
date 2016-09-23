﻿using System;
using MonoBrickFirmware.Display.Dialogs;

namespace MonoBrickFirmware.Display.Menus
{
	public class ItemWithCheckBoxStep : ItemWithCheckBox, IParentItem
	{
		private CheckBoxStep itemStep = null;
		private ItemWithDialog<ProgressDialog> dialogItem;

		public ItemWithCheckBoxStep (string text, bool checkedAtStart, string stepTitle, CheckBoxStep step) : this(text, checkedAtStart, stepTitle, step, null)
		{
			
		}

		public ItemWithCheckBoxStep (string text, bool checkedAtStart, string stepTitle, CheckBoxStep step, Action<bool> OnCheckedChanged) : base(text, checkedAtStart, OnCheckedChanged)
		{
			this.itemStep = step;
			dialogItem = new ItemWithDialog<ProgressDialog>(new ProgressDialog(stepTitle, itemStep));
		}

		public override void OnEnterPressed ()
		{
			itemStep.Checked = this.Checked;
			dialogItem.SetFocus(this);	
		}

		public override void OnHideContent ()
		{
			dialogItem.OnHideContent ();
		}


		#region IParentItem implementation

		public void SetFocus (IChildItem item)
		{
			Parent.SetFocus (item);
		}

		public virtual void RemoveFocus (IChildItem item)
		{
			if (dialogItem.Dialog.Ok)
			{
				this.Checked = !this.Checked;
			}
			Parent.RemoveFocus (item);
		}

		public void SuspendButtonEvents ()
		{
			Parent.SuspendButtonEvents ();
		}

		public void ResumeButtonEvents ()
		{
			Parent.SuspendButtonEvents ();
		}

		#endregion

	}

	public class CheckBoxStep: IStep
	{
		private IStep checkedStep;
		private IStep unCheckStep;

		public CheckBoxStep(IStep checkedStep, IStep unCheckedStep)
		{
			this.checkedStep = checkedStep;
			this.unCheckStep = unCheckedStep;
		}


		public bool Checked{get; set;}
		public string StepText{get {return Checked ? unCheckStep.StepText : checkedStep.StepText;}}
		public string ErrorText{get {return Checked ? unCheckStep.ErrorText : checkedStep.ErrorText;}}
		public string OkText{get {return Checked ? unCheckStep.OkText : checkedStep.OkText;}}
		public bool ShowOkText{get {return Checked ? unCheckStep.ShowOkText : checkedStep.ShowOkText;}}
		public bool Execute ()
		{
			return Checked ? unCheckStep.Execute () : checkedStep.Execute ();
		}
	}
}

