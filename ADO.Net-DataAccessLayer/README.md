# ADO.Net-DataAccessLayer
A more professional Data Access Layer implementation.
This API simplifies the process of using ADO.net in a more organized way,
allowing developers to create a Data-driven applications more rapidly than ever before.

ADODataAccess 

ADODataAccess is API which encapsulates all the implementation to connect and fetch data. It will not only help us to maintain database connectivity related code separately, but will also facilitate to easily replace SQL Database Provider with any other Data Provider as per requirements. We have explicitly returned bases classes DbConnection, DbCommand, DbParameter and DbDataReader instead of SqlConnection, SqlCommand, SqlParameter and SqlDataReader to abstract  database connectivity from implementer.



Using ADODataAccess 
We may recommend to use SqlDataAccessLayer as base class of any other class, ideally your actual DataAccess component.
If you think, you don't need full DataAccess layer, the you can make this concrete class by removing abstract keyword from declaration 
and also make its protected methods to public/internal as per requirements.
