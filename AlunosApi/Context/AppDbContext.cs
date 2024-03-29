﻿using AlunosApi.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AlunosApi.Context
{
    //-- Cabe fazer a instalação do IdentityFrameworkCore

    //-- <IdentityUser> tipo que vai representar
    //o usuario, no caso o user do proprio identity

    public class AppDbContext : IdentityDbContext<IdentityUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<Aluno> Alunos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Aluno>().HasData( // -se nao possui dados, ele inclui
                new Aluno
                {
                    Id = 1, // -neste caso deve-se informar a data
                    Nome = "Maria da Penha",
                    Email = "mariapenha@gmail.com",
                    Idade = 23
                },
                new Aluno
                {
                    Id = 2,
                    Nome = "Manoel Bueno",
                    Email = "manuelbueno@gmail.com",
                    Idade = 22
                }
           );
        }
    }
}
