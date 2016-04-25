/*
 *  "GEDKeeper", the personal genealogical database editor.
 *  Copyright (C) 2009-2016 by Serg V. Zhdanovskih (aka Alchemist, aka Norseman).
 *
 *  This file is part of "GEDKeeper".
 *
 *  This program is free software: you can redistribute it and/or modify
 *  it under the terms of the GNU General Public License as published by
 *  the Free Software Foundation, either version 3 of the License, or
 *  (at your option) any later version.
 *
 *  This program is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *  GNU General Public License for more details.
 *
 *  You should have received a copy of the GNU General Public License
 *  along with this program.  If not, see <http://www.gnu.org/licenses/>.
 */

namespace GKCommon.GEDCOM
{
    public class GEDCOMPointer : GEDCOMTag
    {
        private string fXRef;

        public GEDCOMRecord Value
        {
            get	{
                return base.FindRecord(this.XRef);
            }
            set	{
                this.fXRef = "";
                if (value != null)
                {
                    string xrf = value.XRef;
                    if (string.IsNullOrEmpty(xrf))
                    {
                        xrf = value.NewXRef();
                    }
                    this.XRef = xrf;
                }
            }
        }

        public string XRef
        {
            get { return GEDCOMUtils.CleanXRef(this.fXRef); }
            set { this.fXRef = GEDCOMUtils.EncloseXRef(value); }
        }

        protected override void CreateObj(GEDCOMTree owner, GEDCOMObject parent)
        {
            base.CreateObj(owner, parent);
            this.fXRef = "";
        }

        protected override string GetStringValue()
        {
            return this.fXRef;
        }

        public override bool IsEmpty()
        {
            return (string.IsNullOrEmpty(this.fXRef));
        }

        public override string ParseString(string strValue)
        {
            this.fXRef = "";
            string result = strValue;
            result = GEDCOMUtils.ExtractDelimiter(result, 0);

            if (!string.IsNullOrEmpty(result) && result[0] == '@' && result[1] != '#')
            {
                int pos = result.IndexOf('@', 2);
                if (pos > 0)
                {
                    pos++;
                    this.fXRef = result.Substring(0, pos);
                    result = result.Remove(0, pos);
                }
            }
            return result;
        }

        public override void ReplaceXRefs(XRefReplacer map)
        {
            base.ReplaceXRefs(map);
            this.XRef = map.FindNewXRef(this.XRef);
        }

        public void SetNamedValue(string name, GEDCOMRecord record)
        {
            base.SetName(name);
            this.Value = record;
        }

        public GEDCOMPointer(GEDCOMTree owner, GEDCOMObject parent, string tagName, string tagValue) : base(owner, parent, tagName, tagValue)
        {
        }

        public new static GEDCOMTag Create(GEDCOMTree owner, GEDCOMObject parent, string tagName, string tagValue)
        {
            return new GEDCOMPointer(owner, parent, tagName, tagValue);
        }
    }
}