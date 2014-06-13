function SetGravatar(s_imgId, s_email, i_size){
    jQuery.get("/base/blog4umbraco/GetGravatarImage/" + s_email + ".aspx",
       function(data){
        var avatar = jQuery("#" + s_imgId );
        if(data != ""){
              var src = "http://www.gravatar.com/avatar/" + data + "?s=" + i_size;
              avatar.src(src).show();
          }else{
              avatar.hide();
          }
       });
}

function CreateComment(s_name, s_email, s_website, s_comment, i_pageId){
    jQuery.get("/base/blog4umbraco/CreateComment/" + i_pageId + ".aspx", { name: s_name, email: s_email, website: s_website, comment: s_comment},
       function(data){
          alert("Data Loaded: " + data);
       });
}