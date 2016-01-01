function setTheme(el){
    function request(theme){
        var url = "/dashboard/my/themes/set-default/";
        url += theme;
        return window.getAjaxRequest(url, "POST");
    };

    var theme = $(el).text();
    if(!theme){
        return;
    };
    
    var ajax = request(theme);
    
    ajax.success(function(){
        window.location = window.location;
    });        
};

function loadThemes(){
    function append(theme){
        var item = $("<a onclick='setTheme(this);' class='item' />");
        item.text(theme);
        $(".theme.selector .scrolling.menu").append(item);
    };
    
    function request(){
        var url = "/dashboard/my/themes";
        return window.getAjaxRequest(url);
    };
    
    var ajax = request();
    
    ajax.success(function(response){
        $.each(response, function(){
            append(this);
        });
    });
};

loadThemes();
