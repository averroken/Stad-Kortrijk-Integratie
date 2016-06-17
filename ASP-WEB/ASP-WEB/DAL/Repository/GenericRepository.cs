using ASP_WEB.DAL.Context;
using System.Collections.Generic;
using System.Data.Entity;

/// <summary>
/// GenericRepository
/// </summary>
/// <typeparam name="TEntity">Type of Object to handle</typeparam>
public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
{

    internal IntegratieContext context;
    internal DbSet<TEntity> dbSet;

    /// <summary>
    /// Constructor without context
    /// </summary>
    public GenericRepository()
    {
        this.context = new IntegratieContext();
        this.dbSet = context.Set<TEntity>();
    }
    /// <summary>
    /// Constructor with context
    /// </summary>
    /// <param name="context"></param>
    public GenericRepository(IntegratieContext context)
    {
        this.context = context;
        this.dbSet = context.Set<TEntity>();
    }
    /// <summary>
    /// Gets all
    /// </summary>
    /// <returns></returns>
    public virtual IEnumerable<TEntity> All()
    {
        return dbSet;
    }
    /// <summary>
    /// Gets one byID
    /// </summary>
    /// <param name="id">ID</param>
    /// <returns></returns>
    public virtual TEntity GetByID(object id)
    {
        return dbSet.Find(id);
    }
    /// <summary>
    /// Inserts
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    public virtual TEntity Insert(TEntity entity)
    {
        return dbSet.Add(entity);
    }
    /// <summary>
    /// Deletes by ID
    /// </summary>
    /// <param name="id">ID</param>
    public virtual void Delete(object id)
    {
        TEntity entityToDelete = dbSet.Find(id);
        Delete(entityToDelete);
    }
    /// <summary>
    /// Deletes by Object
    /// </summary>
    /// <param name="entityToDelete">Object</param>
    public virtual void Delete(TEntity entityToDelete)
    {
        if (context.Entry(entityToDelete).State == EntityState.Detached)
        {
            dbSet.Attach(entityToDelete);
        }
        dbSet.Remove(entityToDelete);
    }
    /// <summary>
    /// Update
    /// </summary>
    /// <param name="entityToUpdate"></param>
    public virtual void Update(TEntity entityToUpdate)
    {
        dbSet.Attach(entityToUpdate);
        context.Entry(entityToUpdate).State = EntityState.Modified;
    }
    /// <summary>
    /// Saves Changes in Context
    /// </summary>
    public virtual void SaveChanges()
    {
        context.SaveChanges();
    }
}