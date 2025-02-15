﻿using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace OnGuardCore
{
  public partial class InterestingItemsDialog : Form
  {
    public InterestingItemsDialog(AnalysisResult analysisResult)
    {
      InitializeComponent();

      foreach (var interest in analysisResult.InterestingObjects)
      {
        ListViewItem item = new()
        {
          Text = interest.Area.AOIName
        };

        double screenWidthPercent = Math.Round((100.0 * interest.ObjectRectangle.Width) / BitmapResolution.XResolution, 1, MidpointRounding.AwayFromZero);
        double screenHeightPercent = Math.Round((100.0 * interest.ObjectRectangle.Height) / BitmapResolution.YResolution, 1, MidpointRounding.AwayFromZero);

        item.SubItems.Add(interest.Area.AOIType.ToString());
        item.SubItems.Add(interest.Label);
        item.SubItems.Add(interest.Confidence.ToString());
        item.SubItems.Add(interest.Overlap.ToString());
        item.SubItems.Add(screenWidthPercent.ToString());
        item.SubItems.Add(screenHeightPercent.ToString());

        interestingListView.Items.Add(item);

      }

      foreach (string item in analysisResult.FailureReasons)
      {
        failuresList.Items.Add(item);
      }


    }

    private void DoneButton_Click(object sender, EventArgs e)
    {
      DialogResult = DialogResult.OK;
    }

   
  }
}
