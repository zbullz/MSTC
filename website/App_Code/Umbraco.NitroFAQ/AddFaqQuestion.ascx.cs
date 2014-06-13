using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using umbraco.cms.businesslogic.web;
using umbraco.presentation.nodeFactory;
using umbraco.BusinessLogic;
using umbraco.cms.businesslogic.member;


namespace umbracoFAQ
{
    public partial class AddFaqQuestion : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // fill all values
            if (!IsPostBack)
            {
                lblAddQuestion.Text = questionLabel;
                btnAddQuestion.Text = questionButtonText;
                litTitle.Text = Title;
            }

            // get current node to work with
            Node n = Node.GetCurrent();
            // show the form depending on the page parameter.
            try
            {
                if (n.GetProperty("ShowForm").Value.ToString() == "1")
                    pQuestion.Visible = true;
                else
                    pQuestion.Visible = false;
            }
            catch { }
            
            // check only categories and faq may hold this form.
            if ((n.NodeTypeAlias != "faqCategory") && (n.NodeTypeAlias != "faq"))
            {
                pQuestion.Visible = false;
                setFeedback("This macro is intended to add a question to a Frequently Asked Questions page. Therefor you can only use the features of this macro on either a page with the document type 'faq' or 'faqCategory'. This is done so to prevent someone from adding question nodes to other unintended places.", feedbackStatus.Negative);
            }
        }

        
        #region "properties"
            //bool _showForm = false;
            //public bool showForm
            //{
            //    get { return _showForm; }
            //    set { _showForm = value; }
            //}
            string _thanks = "";
            public string thankyouMessage
            {
                get { return _thanks; }
                set { _thanks = value; }
            }
            public string questionLabel {
                get { return lblAddQuestion.Text; }
                set { lblAddQuestion.Text = value; }
            }
            public string questionButtonText
            {
                get { return btnAddQuestion.Text; }
                set { btnAddQuestion.Text = value; }
            }
            public string Title
            {
                get { return litTitle.Text; }
                set { litTitle.Text = value; }
            }
            enum feedbackStatus  { Negative, Positive };
        #endregion

        #region "methods"
            protected void SubmitQuestion_Click(object sender, EventArgs e)
            {
                hideFeedback();


                // Create a new Question underneath the current category
                Node n = Node.GetCurrent();
                int parentID = n.Id;

                // Get the faqQuestion documenttype
                DocumentType dt = DocumentType.GetByAlias("faqQuestion");

                // Use the "Umbraco" system administrator user as creator (with id:0)
                User u = User.GetUser(0);

                // Create the text used as nodeName
                string questionText = "";
                if (txtAddQuestion.Text.Length > 30)
                    questionText = txtAddQuestion.Text.Substring(0, 30).Trim() + " ...";
                else
                    questionText = txtAddQuestion.Text;

                // Create the document
                Document question = Document.MakeNew(questionText, dt, u, parentID);
                question.getProperty("questionText").Value = txtAddQuestion.Text;

                // Publish the document
                // task.Publish(u);

                // Reflect the publish to the runtime
                umbraco.library.PublishSingleNode(question.Id);

                // thanks 
                txtAddQuestion.Text = "";
                setFeedback(thankyouMessage, feedbackStatus.Positive);


            }

            private void hideFeedback()
            {
                pFeedback.Visible = false;
                lblFeedback.Text = "";
            }
            private void setFeedback(string message, feedbackStatus status)
            {
                pFeedback.CssClass = status.ToString();
                pFeedback.Visible = true;
                lblFeedback.Text = message;
            }
        #endregion

            
    }
}