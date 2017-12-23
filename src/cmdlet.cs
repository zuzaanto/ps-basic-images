using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management.Automation;
using System.Drawing;

namespace psModuleImageEdition
{
    [Cmdlet(VerbsCommon.Get, "imagesSum")]
    public class cmdlet : PSCmdlet
    {
        private string image1SourceCollection;
        private string image2SourceCollection;
        private string operationTypesCollection;

        [Parameter(
            Mandatory = true,
            HelpMessage = "insert path to the first image"
            )]
        [Alias("Image1", "Source1")]
        public string image1Source
        {
            get { return image1SourceCollection; }
            set { image1SourceCollection = value; }
        }
        [Parameter(
            Mandatory = true,
            HelpMessage = "insert path to the second image"
            )]
        [Alias("Image2", "Source2")]
        public string image2Source
        {
            get { return image2SourceCollection; }
            set { image2SourceCollection = value; }
        }
        [Parameter(
            Mandatory = true,
            HelpMessage = "choose type of operation"
            )]
        [Alias("type", "operation")]
        public string operationType
        {
            get { return operationTypesCollection; }
            set { operationTypesCollection = value; }
        }

        protected override void BeginProcessing()
        {
            base.BeginProcessing();
        }
        protected override void ProcessRecord()
        {

            Bitmap img1 = ImageFunctions.Load(image1Source);
            Bitmap img2 = ImageFunctions.Load(image2Source);
            Bitmap result = null;
            if (operationType == "add" || operationType == "Add")
            {
                result = img1.ArithmeticBlend(img2, ImageFunctions.ColorCalculationType.Add);
            }
            if (operationType == "subtract" || operationType == "difference")
            {
                result = img1.ArithmeticBlend(img2, ImageFunctions.ColorCalculationType.Difference);
            }
            if (operationType == "average" || operationType == "avg")
            {
                result = img1.ArithmeticBlend(img2, ImageFunctions.ColorCalculationType.Average);
            }
            WriteObject(result);
        }
        
        protected override void EndProcessing()
        {
            base.EndProcessing();
        }
    }
}
