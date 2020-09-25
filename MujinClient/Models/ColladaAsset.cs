using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MujinClient.Models
{

    // NOTE: Generated code may require at least .NET Framework 4.5 or .NET Core/Standard 2.0.
    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.collada.org/2008/03/COLLADASchema")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.collada.org/2008/03/COLLADASchema", IsNullable = false)]
    public partial class COLLADA
    {

        private COLLADAAsset assetField;

        private string versionField;

        /// <remarks/>
        public COLLADAAsset asset
        {
            get
            {
                return this.assetField;
            }
            set
            {
                this.assetField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.collada.org/2008/03/COLLADASchema")]
    public partial class COLLADAAsset
    {

        private System.DateTime createdField;

        private string keywordsField;

        private System.DateTime modifiedField;

        private object unitField;

        private string up_axisField;

        private COLLADAAssetContributor contributorField;

        /// <remarks/>
        public System.DateTime created
        {
            get
            {
                return this.createdField;
            }
            set
            {
                this.createdField = value;
            }
        }

        /// <remarks/>
        public string keywords
        {
            get
            {
                return this.keywordsField;
            }
            set
            {
                this.keywordsField = value;
            }
        }

        /// <remarks/>
        public System.DateTime modified
        {
            get
            {
                return this.modifiedField;
            }
            set
            {
                this.modifiedField = value;
            }
        }

        /// <remarks/>
        public object unit
        {
            get
            {
                return this.unitField;
            }
            set
            {
                this.unitField = value;
            }
        }

        /// <remarks/>
        public string up_axis
        {
            get
            {
                return this.up_axisField;
            }
            set
            {
                this.up_axisField = value;
            }
        }

        /// <remarks/>
        public COLLADAAssetContributor contributor
        {
            get
            {
                return this.contributorField;
            }
            set
            {
                this.contributorField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.collada.org/2008/03/COLLADASchema")]
    public partial class COLLADAAssetContributor
    {

        private string authorField;

        private string authoring_toolField;

        /// <remarks/>
        public string author
        {
            get
            {
                return this.authorField;
            }
            set
            {
                this.authorField = value;
            }
        }

        /// <remarks/>
        public string authoring_tool
        {
            get
            {
                return this.authoring_toolField;
            }
            set
            {
                this.authoring_toolField = value;
            }
        }
    }


}