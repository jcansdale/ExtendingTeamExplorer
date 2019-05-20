This sample demonstrates several of the Team Explorer extensibility points.

  - Team Explorer Page

      This sample implements a Recent Changes page that displays recent checkins for the current team project.

  - Team Explorer Section

      This sample implements two sections that extend the Recent Changes page.  The sections show the most
	  recent checkins by the current user ("My Changes") and all users ("All Changes").

  - Team Explorer Navigation Item

      This sample currently does not implement a navigation item.  Navigation items are very similar to navigation
	  links.  A navigation item sample will be added in the future.

  - Team Explorer Navigation Link

      This sample implements a Recent Changes navigation link that takes the user to the Recent Changes page.


Existing Team Explorer pages can also be extended with additional sections.  This sample adds a new section to
the Pending Changes page titled "Selected File Info".  The section displays additional information about the
first selected item in the Included Changes section.  If multiple items are selected, only info for the first
item selected will be shown.