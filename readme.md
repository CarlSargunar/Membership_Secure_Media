# Member Test

This demo shows how to protecte media in Umbraco 13 so only logged in member can access it. It's a bit hacky, but it works. 

On the homepage is an Image and a PDF, both of which only work when a member is logged in.

It relies on a surface controller which checks if the user is logged in and then returns the media. All relevant cost is in [SecureMediaController.cs](/MemberTest/Code/SecureMediaController.cs)

To run the demo, just clone it and use 

    dotnet run --project "MemberTest"

## Usage

Sample usage below. In order to know whether media NEEDS to be protected or not, you can make your own rules. Add a property to the media, or whatever. 

SUGGESTION! - create a tag helper to render the media with the right URL.

    <div class="container">
        <h2>Secured Media</h2>
        <p>Below is a secured media item that is only accessible to authenticated users.</p>
        <img src="/umbraco/surface/SecureMedia/getmediabyid?id=1139" />
        <hr />
        <a href="/umbraco/surface/SecureMedia/getmediabyid?id=1166">PDF Sample</a>
    </div>

## Setup

    dotnet new install Umbraco.Templates::13.4.1 --force

    dotnet new umbraco --force -n "MemberTest" --friendly-name "Administrator" --email "admin@example.com" --password "1234567890" --development-database-type SQLite

    #Add Packages
    dotnet add "MemberTest" package Umbraco.TheStarterKit --version 13.0.0

    dotnet run --project "MemberTest"

## Test User

Or you can register yourself

    Username: test@test.com
    Password: Pa55word!!