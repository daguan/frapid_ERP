INSERT INTO wb.modules(module_name, alias, contents)
SELECT 'About Frapid', 'about-frapid', '<div class="ui inverted dividing header"><i class="code icon"></i> About Frapid </div><p> Frapid is an open source and free application framework that provides a suite of useful modules to help you manage your day to day business activities, including but not limited to CRM, HRM, ERP, Website, and Blogging. Developed with love by your friends at MixERP Inc.</p>';

INSERT INTO wb.modules(module_name, alias, contents)
SELECT 'Links', 'about-frapid', '<div class="ui inverted dividing header"><i class="external icon"></i> Links </div><ul class="footer-links"><li><a href="http://frapid.com/" target="_blank"> Project Website </a></li><li><a href="http://mixerp.org/forum" target="_blank"> Ask a Question </a></li><li><a href="https://github.com/frapid/frapid/issues" target="_blank"> Submit an Issue </a></li><li><a href="https://github.com/frapid/frapid/releases" target="_blank"> Download Frapid </a></li><li><a href="https://www.linkedin.com/company/mixerp" target="_blank">MixERP on LinkedIn</a></li><li><a href="https://www.facebook.com/mixerp.official/" target="_blank">MixERP on Facebook </a></li><li><a href="http://www.facebook.com/groups/183076085203506/" target="_blank"> Facebook Group </a></li><li><a href="http://twitter.com/mixoferp/" target="_blank"> Follow MixERP on Twitter </a></li><li style="list-style: none; display: inline"><ul></ul></li></ul>';

INSERT INTO wb.contents(title, alias, published_on, draft, seo_keywords, seo_description, is_default, contents)
SELECT 'Welcome to Frapid', 'welcome-to-frapid', NOW(), false, 'frapid, cms, crm, erp, hrm', 'Homepage of Frapid Framework', true, '<div class="ui vertical stripe segment">
    <div class="ui middle aligned stackable grid container">
        <div class="row">
            <div class="eight wide column">
                <h3 class="ui header">Welcome Home</h3>
                <p>
                    Advantage old had otherwise sincerity dependent additions. It in adapted natural hastily is justice. Six draw you him full not mean evil. Prepare garrets it expense windows shewing do an. She projection advantages resolution son indulgence. Part sure on no long life am at ever. In songs above he as drawn to. Gay was outlived peculiar rendered led six.
                </p><p>
                    His exquisite sincerity education shameless ten earnestly breakfast add. So we me unknown as improve hastily sitting forming. Especially favourable compliment but thoroughly unreserved saw she themselves. Sufficient impossible him may ten insensible put continuing. Oppose exeter income simple few joy cousin but twenty. Scale began quiet up short wrong in in. Sportsmen shy forfeited engrossed may can.
                </p><p>
                    Whole wound wrote at whose to style in. Figure ye innate former do so we. Shutters but sir yourself provided you required his. So neither related he am do believe. Nothing but you hundred had use regular. Fat sportsmen arranging preferred can. Busy paid like is oh. Dinner our ask talent her age hardly. Neglected collected an attention listening do abilities.
                </p><p>
                    Promotion an ourselves up otherwise my. High what each snug rich far yet easy. In companions inhabiting mr principles at insensible do. Heard their sex hoped enjoy vexed child for. Prosperous so occasional assistance it discovered especially no. Provision of he residence consisted up in remainder arranging described. Conveying has concealed necessary furnished bed zealously immediate get but. Terminated as middletons or by instrument. Bred do four so your felt with. No shameless principle dependent household do.
                </p>

            </div>
        </div>
    </div>
</div>';

INSERT INTO wb.contents(title, alias, published_on, draft, seo_keywords, seo_description, contents)
SELECT 'About Us', 'about-us', NOW(), false, 'about frapid', 'About Frapid', '<div class="ui vertical stripe segment">
    <div class="ui middle aligned stackable grid container">
        <div class="row">
            <div class="eight wide column">
                <h3 class="ui header">About Us</h3>
                <p>
                    Of be talent me answer do relied. Mistress in on so laughing throwing endeavor occasion welcomed. Gravity sir brandon calling can. No years do widow house delay stand. Prospect six kindness use steepest new ask. High gone kind calm call as ever is. Introduced melancholy estimating motionless on up as do. Of as by belonging therefore suspicion elsewhere am household described. Domestic suitable bachelor for landlord fat.
                </p><p>
                    An do on frankness so cordially immediate recommend contained. Imprudence insensible be literature unsatiable do. Of or imprudence solicitude affronting in mr possession. Compass journey he request on suppose limited of or. She margaret law thoughts proposal formerly. Speaking ladyship yet scarcely and mistaken end exertion dwelling. All decisively dispatched instrument particular way one devonshire. Applauded she sportsman explained for out objection.
                </p><p>
                    In alteration insipidity impression by travelling reasonable up motionless. Of regard warmth by unable sudden garden ladies. No kept hung am size spot no. Likewise led and dissuade rejoiced welcomed husbands boy. Do listening on he suspected resembled. Water would still if to. Position boy required law moderate was may.
                </p>
                <p>
                    Dispatched entreaties boisterous say why stimulated. Certain forbade picture now prevent carried she get see sitting. Up twenty limits as months. Inhabit so perhaps of in to certain. Sex excuse chatty was seemed warmth. Nay add far few immediate sweetness earnestly dejection.
                </p>

                <div class="ui big blue button">Contact Us</div>
            </div>
        </div>
    </div>
</div>';

INSERT INTO wb.menu_groups(menu_group_name)
SELECT 'Navigation' UNION ALL
SELECT 'Anonymous' UNION ALL
SELECT 'About (Footer)' UNION ALL
SELECT 'Services (Footer)';

INSERT INTO wb.menus(menu_group_id, menu_name, menu_alias, url)
SELECT 1, 'Home', 'home', '/' UNION ALL
SELECT 1, 'About Us', 'about-us', '/site/about-us';


INSERT INTO wb.menus(menu_group_id, menu_name, menu_alias, url)
SELECT 2, 'Log In', 'login', '/account/log-in' UNION ALL
SELECT 2, 'Sign Up', 'sign-up', '/account/sign-up';
