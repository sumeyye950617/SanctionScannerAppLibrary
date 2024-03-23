# SanctionScannerAppLibrary
 ## We made a Library application using Net Core, Mvc, EF Core, HTML5, CSS, JQUERY
 <p> In this application we used the Ef Core DB first approach. For this we installed some packages. These packages are:

  <ul>
  <li>Microsoft.EntityFrameworkCore.SqlServer</li>
  <li>Microsoft.EntityFrameworkCore.Tools</li>
  <li>Microsoft.EntityFrameworkCore.Design 
</ul>
 </p>
<br/>
  <p>Various packages were installed for the search process in our project:
    <ul>
  <li>X.PagedList.Mvc.Core</li>
  <li> X.PagedList</li>
</ul></p> 
<br/>
  <p>The library performed Serilog operations in our application. These were recorded in a text file. The packages installed for this:
    <ul>
  <li>Serilog.Sinks.File</li>
  <li>Serilog.AspNet.Core</li>
  <li>Serilog.Sinks. Debug </li>
  <li>Serilog.Settings.Congiguration</li>
</ul>
  </p>
<br/>
<h3>What is the Project Purpose??</h3>
<h3> What to Pay Attention to?</h3>
<p>There is a system where users can borrow books from the library and return them. On the listing screen of the books page, there is an interface where you can see whether the books exist, their picture, category and author. Search and pagination operations are also available here. Adding books can also be done. Author, Category, addition, deletion and update operations were also carried out. There was a critical point in our project. When the user bought a book from the library, the book should be written as "not available" since it was no longer available and other users should not be able to buy this book. <em><strong>Trigger</strong></em> was used for this. When the user bought the book, "it is available" In this way, the "not" situation was created. In case the user returns the book, the library should be arranged so that the book is available. For this,  <em><strong>Trigger</strong></em> was used. In this way, the current situation in the library could change while the user was shopping for a book. </p>
  
  
  
