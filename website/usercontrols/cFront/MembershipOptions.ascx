<%@ Control Language="C#" AutoEventWireup="true" CodeFile="MembershipOptions.ascx.cs" Inherits="usercontrols_cFront_MembershipOptions" %>

    <div class="member-options">
        <div class="form-group">
            <h3 class="col-sm-12">Membership options</h3>
        </div>
        <div class="form-group">
            <label for="Name" class="col-sm-2 control-label"><b>Membership*</b></label>
            <div class="col-sm-10">
                <div class="radio">
                    <label>
                        <input type="radio" name="membership" id="membership1" value="individual" />
                        Individual membership - &pound;40
                    </label>
                </div>
                <div class="radio">
                    <label>
                        <input type="radio" name="membership" id="membership2" value="couple" />
                        Couple membership - &pound;35
                                                <br />
                        <i>Only select this option if your partner will also be renewing their membership - The membership secretary will be checking!</i>
                    </label>
                </div>
                <div class="radio">
                    <label>
                        <input type="radio" name="membership" id="membership3" value="concession" />
                        Unemployed/full-time student (18 years or above) - &pound;30
                    </label>
                </div>
            </div>
        </div>
        <div class="form-group">
            <label for="contact-email" class="col-sm-2 control-label"><b>Optional extras</b></label>
            <div class="col-sm-10">
                <div class="checkbox">
                    <label>
                        <input type="checkbox" value="swim-subs-jan-june">
                        Swim subs January to June - &pound;30
                    </label>
                </div>
                <div class="checkbox">
                    <label>
                        <input type="checkbox" value="swim-subs-july-dec">
                        Swim subs July to December - &pound;30
                    </label>
                </div>
                <div class="checkbox">
                    <label>
                        <input type="checkbox" value="swim-subs-july-dec">
                        Spin/Core subs April to Sept - &pound;20
                    </label>
                </div>
                <div class="checkbox">
                    <label>
                        <input type="checkbox" value="swim-subs-july-dec">
                        Turbo/Core subs Oct to March - &pound;20
                    </label>
                </div>
            </div>
        </div>
        <div class="form-group">
            <label for="contact-message" class="col-sm-2 control-label"><b>Open water swimming indemnity waiver*</b></label>
            <div class="col-sm-10">
                <p><a href="http://midsussextriclub.com/media/19048/MSTCArdinglyReservoirindemnityterms_2012.doc" target="_blank">Click here to view the Open Water Swimming Indemnity Document</a></p>
                <div class="radio">
                    <label>
                        <input type="radio" name="indemnity-waiver" id="indemnity-waiver1" value="accept" />
                        I have read and understand the open water swimming indemnity document.
                                                <br />
                        I agree to and accept them without qualification.
                    </label>
                </div>
                <div class="radio">
                    <label>
                        <input type="radio" name="indemnity-waiver" id="indemnity-waiver2" value="reject" />
                        I do not accept the open water swimming indemnity document.
                                                <br />
                        I understand I will not be elligible to take part in club open water swim sessions for this membership year.
                    </label>
                </div>
            </div>
        </div>
        <div class="form-group">
            <label class="col-sm-2 control-label"><b>Volunteering agreement*</b></label>
            <div class="col-sm-7">
                <div class="checkbox">
                    <label>
                        <input type="checkbox" value="swim-subs-jan-june">
                        I agree to give at least 5 hours of my time to assist in duties for the club during the current year.
                    </label>
                </div>
            </div>
        </div>
    </div>
